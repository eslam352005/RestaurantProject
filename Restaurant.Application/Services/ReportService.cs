using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Report;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Services
{
    public class ReportService(IApplicationDbcontext _context) : IReportService
    {
        public async Task<ResponseDto<IEnumerable<SalesReportDto>>> GetSalesReportAsync(int? branchId, DateTime? from = null, DateTime? to =null)
        {
            if ((from == null) && (to == null))
            {
                from = DateTime.Now.AddDays(-30);
                to = DateTime.Now;
            }
            if ((from != null) && (to == null))
            {
                to = DateTime.Now;
            }
            if ((from == null) && (to != null))
            {
                to = DateTime.Now;
                from = to.Value.AddDays(-30);
            }

            var orders = await _context.Orders
                .Where(o => (o.Status == OrderStatus.Served)
                && (!branchId.HasValue || o.BranchId == branchId)
                &&(o.CreatedAt>= from.Value)
                && (o.CreatedAt <= to.Value))
                .GroupBy(o => o.CreatedAt.Date)
                .Select(oi => new SalesReportDto
                {
                    TotalSales = oi.Sum(i => i.TotalAmount),
                    OrdersCount = oi.Count(),
                    Date = oi.Key
                })
                .OrderByDescending(i=>i.Date)
                .ToListAsync();

           
            return ResponseHandler.Success<IEnumerable<SalesReportDto>>(orders, "Sales Report retrieved successfully");
        }

        public async Task<ResponseDto<IEnumerable<StaffPerformanceDto>>> GetStaffPerformanceReportAsync(int? branchId)
        {
            var orders = await _context.Orders
                .Where(o => (!branchId.HasValue || o.BranchId == branchId)
                && (o.Status == OrderStatus.Served))
                .GroupBy(o => new { o.WaiterId, o.Waiter.FullName })
                .Select(o => new StaffPerformanceDto
                {
                    StaffId = o.Key.WaiterId,
                    FullName = o.Key.FullName,
                    OrdersHandled = o.Count(),
                    TotalSalesHandled = o.Sum(x=> x.TotalAmount)
                })
                .OrderByDescending(x => x.TotalSalesHandled)
                .ToListAsync();

            return ResponseHandler.Success<IEnumerable<StaffPerformanceDto>>(orders, "Staff Performance Report retrieved successfully");
        }

        public async Task<ResponseDto<IEnumerable<TopItemDto>>> GetTopItemsReportAsync(int? branchId, int limit =5)
        {
            var orders = await _context.OrderItems
                        .Where(oi => (oi.Order.Status == OrderStatus.Served)
                        && (!branchId.HasValue || oi.Order.BranchId == branchId))
                        .GroupBy(oi => new { oi.MenuItemId, oi.MenuItem.Name })   
                        .Select(g => new TopItemDto
                        {
                            MenuItemId = g.Key.MenuItemId,
                            Name = g.Key.Name,
                            TotalOrdered = g.Sum(x => x.Quantity),
                            TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
                        })
                        .OrderByDescending(x => x.TotalOrdered)
                        .Take(limit)
                        .ToListAsync();

           
            return ResponseHandler.Success<IEnumerable<TopItemDto>>(orders, "Top Items Report retrieved successfully");
        }
    }
}
