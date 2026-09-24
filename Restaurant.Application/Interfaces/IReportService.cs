using Restaurant.Application.DTOs.Report;
using Restaurant.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IReportService
    {
        public Task<ResponseDto<IEnumerable<SalesReportDto>>> GetSalesReportAsync(int? branchId ,DateTime? from = null  , DateTime? to = null);
        public Task<ResponseDto<IEnumerable<StaffPerformanceDto>>> GetStaffPerformanceReportAsync(int? branchId);
        public Task<ResponseDto<IEnumerable<TopItemDto>>> GetTopItemsReportAsync(int? branchId,int limit =5);

    }
}
