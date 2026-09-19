using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.DTOs.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface ITableService
    {
        public Task<ResponseDto<IEnumerable<TableDto>>> GetAllTablesAsync(int? branchId);
        public Task<ResponseDto<TableDto>> GetTableByIdAsync(int id);
        public Task<ResponseDto<TableDto>> CreateTableAsync(CreateTableDto tableDto);
        public Task<ResponseDto<TableDto>> UpdateTableAsync(int id, UpdateTableDto tableDto);
        public Task<ResponseDto<TableDto>> UpdateTableStatusAsync(int id, UpdateTableStatusDto statusDto);
        public Task<ResponseDto<bool>> DeleteTableAsync(int id);
    }
}
