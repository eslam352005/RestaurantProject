using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Order;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Services
{
    public class OrderService(IApplicationDbcontext _context,IKitchenNotifier _kitchenNotifier,IMapper _mapper) : IOrderservice
    {
        public async Task<ResponseDto<OrderDto>> ChangeOrderItemStatusAsync(int orderid, int itemId, UpdateOrderItemStatusDto statusDto)
        {
            var order = await _context.Orders
                        .Include(o => o.Items)
                        .FirstOrDefaultAsync(o => o.Id == orderid);
            if (order == null)
            {
                return ResponseHandler.NotFound<OrderDto>($"Order With Id {orderid} is not found");
            }
            var item = order.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return ResponseHandler.NotFound<OrderDto>($"Item With Id {itemId} is not found");
            }
            if(item.Status == OrderItemStatus.Ready)
            {
                return ResponseHandler.BadRequest<OrderDto>("Cannot change status of a completed Order Item");
            }
            if (order.Status == OrderStatus.Served || order.Status == OrderStatus.Cancelled)
            {
                return ResponseHandler.BadRequest<OrderDto>("Cannot change item status on a completed or cancelled order");
            }
            item.Status = statusDto.Status;

            if (!order.Items.Any(i => i.Status != OrderItemStatus.Ready))
            {
                order.Status = OrderStatus.Ready;
            }
            await _context.SaveChangesAsync();
           await _kitchenNotifier.NotifyItemStatusChanged(order.BranchId, order.Id, itemId, statusDto.Status);

            var orderDto = _mapper.Map<OrderDto>(order);

            return ResponseHandler.Success(orderDto, "Order Items Updated Successfully");
        }

        public async Task<ResponseDto<OrderDto>> ChangeOrderStatusAsync(int id, UpdateOrderStatusDto statusDto)
        {
            var order = await _context.Orders
                        .Include(o => o.Items)
                        .Include(o => o.Table)
                        .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return ResponseHandler.NotFound<OrderDto>($"Order With Id {id} is not found");
            }
            if (order.Status == OrderStatus.Served || order.Status == OrderStatus.Cancelled)
            {
                return ResponseHandler.BadRequest<OrderDto>("Cannot change status of a completed or cancelled order");
            }

            var validTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
             {
                { OrderStatus.Pending,    new List<OrderStatus> { OrderStatus.Preparing, OrderStatus.Cancelled } },
                { OrderStatus.Preparing,  new List<OrderStatus> { OrderStatus.Ready, OrderStatus.Cancelled } },
                { OrderStatus.Ready,      new List<OrderStatus> { OrderStatus.Served, OrderStatus.Cancelled } },
              };

            if (!validTransitions.ContainsKey(order.Status) ||
                !validTransitions[order.Status].Contains(statusDto.Status))
            {
                return ResponseHandler.BadRequest<OrderDto>($"Cannot change status from {order.Status} to {statusDto.Status}");
            }

            if (statusDto.Status == OrderStatus.Cancelled)
            {
                var hasOtherActiveOrders = await _context.Orders
                    .AnyAsync(o => o.TableId == order.TableId
                                && o.Id != order.Id
                                && o.Status != OrderStatus.Served
                                && o.Status != OrderStatus.Cancelled);

                if (!hasOtherActiveOrders)
                    order.Table.Status = TableStatus.Available;
            }

            if (statusDto.Status == OrderStatus.Served)
            {
                
                var notReadyItems = order.Items.Any(i => i.Status != OrderItemStatus.Ready);
                if (notReadyItems)
                    return ResponseHandler.BadRequest<OrderDto>("Cannot serve an order with items not yet ready");

                var hasOtherActiveOrders = await _context.Orders
                    .AnyAsync(o => o.TableId == order.TableId
                                && o.Id != order.Id
                                && o.Status != OrderStatus.Served
                                && o.Status != OrderStatus.Cancelled);

                if (!hasOtherActiveOrders)
                    order.Table.Status = TableStatus.Available;
            }

            order.Status = statusDto.Status;
            await _context.SaveChangesAsync();
            await _kitchenNotifier.NotifyOrderStatusChanged(order.BranchId, order.Id, statusDto.Status);
            var orderDto = _mapper.Map<OrderDto>(order);
            return ResponseHandler.Success(orderDto, "Order Status Updated Successfully");
        }

        public async Task<ResponseDto<OrderDto>> CreateOrderAsync(int waiterId,CreateOrderDto createOrderDto)
        {
            var table = await _context.Tables.FindAsync(createOrderDto.TableId);
            if(table == null)
            {
                return ResponseHandler.NotFound<OrderDto>($"Table with Id:{createOrderDto.TableId} Not Found");
            }
            if(table.Status == TableStatus.Occupied)
            {
                return ResponseHandler.BadRequest<OrderDto>($"Table with Id:{createOrderDto.TableId} Is Occupied");
            }
            if (table.Status == TableStatus.Reserved)
            {
                return ResponseHandler.BadRequest<OrderDto>($"Table with Id:{createOrderDto.TableId} Is Reserved");
            }
            if (createOrderDto.Items == null || !createOrderDto.Items.Any())
                return ResponseHandler.BadRequest<OrderDto>("Order must contain at least one item");
            
            var items = new List<OrderItem>();
           foreach(var item in createOrderDto.Items)
            {
                var menuItem = await _context.MenuItems.Where(i=>(i.Id ==item.MenuItemId) && (i.IsAvailable) && (i.BranchId == createOrderDto.BranchId)).FirstOrDefaultAsync();
                if(menuItem == null)
                {
                    return ResponseHandler.NotFound<OrderDto>($"Menuitem with Id:{item.MenuItemId} Not Found");
                }
                var orderItem = new OrderItem()
                {
                    MenuItemId = menuItem.Id,
                    Quantity = item.Quantity,
                    Notes = item.Notes,
                    UnitPrice = menuItem.Price,
                    Status = OrderItemStatus.Pending,

                };
                items.Add(orderItem);
            }
            var order = new Order(OrderStatus.Pending, createOrderDto.TableId, createOrderDto.BranchId, waiterId);
            foreach (var item in items)
            {
                order.Items.Add(item);
            }
            order.TotalAmount = items.Sum(i => i.UnitPrice * i.Quantity);
            await _context.Orders.AddAsync(order);

            table.Status = TableStatus.Occupied;
           
            await _context.SaveChangesAsync();

           

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.TableNumber = table.TableNumber;
            await _kitchenNotifier.NotifyOrderReceivedAsync(createOrderDto.BranchId, orderDto);

            return ResponseHandler.Success(orderDto, "Order Created Succesfuly");
        }

        public async Task<ResponseDto<OrderDto>> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders
                        .Include(o => o.Table)
                        .FirstOrDefaultAsync(o=> o.Id == id);
            if (order == null)
            {
                return ResponseHandler.NotFound<OrderDto>($"Order With Id {id} is not found");
            }
            if(order.Status == OrderStatus.Served)
            {
                return ResponseHandler.BadRequest<OrderDto>("Cannot Cancel Completed  order");
            }
            order.Status = OrderStatus.Cancelled;

            var hasOtherActiveOrders = await _context.Orders
                    .AnyAsync(o => o.TableId == order.TableId
                                && o.Id != order.Id
                                && o.Status != OrderStatus.Served
                                && o.Status != OrderStatus.Cancelled);

                if (!hasOtherActiveOrders)
                    order.Table.Status = TableStatus.Available;
            await _kitchenNotifier.NotifyOrderStatusChanged(order.BranchId, order.Id, OrderStatus.Cancelled);
            await _context.SaveChangesAsync();
            var orderDto = _mapper.Map<OrderDto>(order);
            return ResponseHandler.Success(orderDto, "Order Status Updated Successfully");
        }

        public async Task<ResponseDto<OrderDto>> GetOrderByIdAsync(int id)
        {
            var order = await  _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.Table)
                .Include(o => o.Waiter)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o=> o.Id == id);
            if(order == null)
            {
                return ResponseHandler.NotFound<OrderDto>($"Order With Id {id} is not found");
            }
            var orderDto = _mapper.Map<OrderDto>(order);
            return ResponseHandler.Success(orderDto, "Order Retrieved Successfully");
        }

        public async Task<ResponseDto<IEnumerable<OrderDto>>> GetOrdersAsync(int? branchId, OrderStatus? status, DateTime? date)
        {
            var orders = await _context.Orders
                .Include(o=>o.Branch)
                .Include(o => o.Table)
                .Include(o => o.Waiter)
                .Include(o => o.Items)
                .Where(o => (!branchId.HasValue || o.BranchId == branchId) &&
                (!status.HasValue || o.Status == status) &&
                (!date.HasValue || o.CreatedAt.Date == date.Value.Date))
                .ToListAsync();
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return ResponseHandler.Success(ordersDto, "Orders Retrieved Successfully");
        }

        public async Task<ResponseDto<OrderDto>> UpdateOrderItemsAsync(int id, UpdateOrderItemsDto itemsDto)
        {
            var order = await _context.Orders
                       .Include(o => o.Items)
                       .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return ResponseHandler.NotFound<OrderDto>($"Order With Id {id} is not found");
            }
            if(order.Status != OrderStatus.Pending && order.Status != OrderStatus.Preparing)
            {
                return ResponseHandler.BadRequest<OrderDto>($"Can't Update Order Items");
            }
            var newItems = new List<OrderItem>();
            foreach(var item in itemsDto.Items)
            {
                var menuItem = await _context.MenuItems.Where(i => (i.Id == item.MenuItemId) && (i.IsAvailable) && (i.BranchId == order.BranchId)).FirstOrDefaultAsync();
                if (menuItem == null)
                {
                    return ResponseHandler.NotFound<OrderDto>($"Menuitem with Id:{item.MenuItemId} Not Found");
                }
                var orderItem = new OrderItem()
                {
                    MenuItemId = menuItem.Id,
                    Quantity = item.Quantity,
                    Notes = item.Notes,
                    UnitPrice = menuItem.Price,
                    Status = OrderItemStatus.Pending,

                };
                newItems.Add(orderItem);
            }

            order.Items.Clear();
            foreach (var item in newItems)
            {
                order.Items.Add(item);
            }
            order.TotalAmount = newItems.Sum(i => i.UnitPrice * i.Quantity);
            await _context.SaveChangesAsync();
            var orderDto = _mapper.Map<OrderDto>(order);

           await _kitchenNotifier.NotifyUpdatedOrderReceivedAsync(order.BranchId, orderDto);

            return ResponseHandler.Success(orderDto, "Order Items Updated Successfully");
        }
    }
}
