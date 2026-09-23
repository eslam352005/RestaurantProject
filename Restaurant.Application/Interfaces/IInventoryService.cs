using Restaurant.Application.DTOs.Inventory;
using Restaurant.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IInventoryService
    {
        public Task<ResponseDto<IEnumerable<InventoryDto>>> GetInventoryAsync(int? branchId);
        public Task<ResponseDto<InventoryDto>> CreateInventoryItemAsync(CreateInventoryDto dto);
        public Task<ResponseDto<InventoryDto>> UpdateInventoryItemAsync(int id,UpdateInventoryDto dto);
        public Task<ResponseDto<InventoryDto>> AdjustStockAsync(int id, AdjustStockDto dto);
        public Task<ResponseDto<IEnumerable<InventoryDto>>> GetLowStockAlertsAsync(int? branchId);

    }
}
