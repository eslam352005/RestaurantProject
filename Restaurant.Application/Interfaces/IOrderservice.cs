using Restaurant.Application.DTOs.Order;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IOrderservice
    {
        public Task<ResponseDto<OrderDto>> CreateOrderAsync(int waiterId,CreateOrderDto createOrderDto);
        public Task<ResponseDto<IEnumerable<OrderDto>>> GetOrdersAsync(int? branchId,OrderStatus? status,DateTime? date);
        public Task<ResponseDto<OrderDto>> GetOrderByIdAsync(int id);
        public Task<ResponseDto<OrderDto>> UpdateOrderItemsAsync(int id,UpdateOrderItemsDto itemsDto);
        public Task<ResponseDto<OrderDto>> ChangeOrderStatusAsync(int id,UpdateOrderStatusDto statusDto);
        public Task<ResponseDto<OrderDto>> ChangeOrderItemStatusAsync(int orderid,int itemId, UpdateOrderItemStatusDto statusDto);
        public Task<ResponseDto<OrderDto>> DeleteOrderAsync(int id);
    }
}
