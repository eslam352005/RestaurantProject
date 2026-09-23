using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Inventory;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Services
{
    public class InventoryService(IApplicationDbcontext _context, IMapper _mapper,IKitchenNotifier _notifier) : IInventoryService
    {
        public async Task<ResponseDto<InventoryDto>> AdjustStockAsync(int id, AdjustStockDto dto)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return ResponseHandler.NotFound<InventoryDto>($"Inventory with id {id} is not found ");
            }
            if(inventory.QuantityAvailable+dto.Amount < 0)
            {
                return ResponseHandler.BadRequest<InventoryDto>("Current Quantity is less than amount you want to Deduct ");
            }
            inventory.QuantityAvailable += dto.Amount;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<InventoryDto>(inventory);

            if (inventory.QuantityAvailable <= inventory.MinimumThreshold)
            {
                await _notifier.MinimumThresholdAlertasync(inventory.BranchId, result);
            }

            return ResponseHandler.Success(result, "Inventory Stock Updated succsessfully");

        }

        public async Task<ResponseDto<InventoryDto>> CreateInventoryItemAsync(CreateInventoryDto dto)
        {
            var branch = await _context.Branches.Include(b=>b.InventoryItems).FirstOrDefaultAsync(b=>b.Id == dto.BranchId);
            if(branch == null)
            {
                return ResponseHandler.NotFound<InventoryDto>($"Branch with id {dto.BranchId} is not found");
            }

            if (branch.InventoryItems.Any(i => i.ItemName.Trim().ToLower() == dto.ItemName.Trim().ToLower()))
            {
                return ResponseHandler.BadRequest<InventoryDto>($"Duplicate Item Name Found");
            }
            var inventoryDto = _mapper.Map<Inventory>(dto);
            await _context.Inventories.AddAsync(inventoryDto);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<InventoryDto>(inventoryDto);
            return ResponseHandler.Success(result, "Inventory Created succsessfully");
        }

        public async Task<ResponseDto<IEnumerable<InventoryDto>>> GetInventoryAsync(int? branchId)
        {
            var inventories = await _context.Inventories
                 .Where(i => (!branchId.HasValue || i.BranchId == branchId)).ToListAsync();
            var inventoriesDto = _mapper.Map<IEnumerable<InventoryDto>>(inventories);
            return ResponseHandler.Success(inventoriesDto, "Inventories Retrieved succsessfully");
        }

        public async Task<ResponseDto<IEnumerable<InventoryDto>>> GetLowStockAlertsAsync(int? branchId)
        {
            var inventories = await _context.Inventories.Where(i => (!branchId.HasValue || i.BranchId == branchId) && (i.QuantityAvailable <= i.MinimumThreshold)).ToListAsync();
            var inventoriesDto = _mapper.Map<IEnumerable<InventoryDto>>(inventories);
            return ResponseHandler.Success(inventoriesDto, "Inventories With low stock Retrieved succsessfully");
        }

        public async Task<ResponseDto<InventoryDto>> UpdateInventoryItemAsync(int id, UpdateInventoryDto dto)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if(inventory == null)
            {
                return ResponseHandler.NotFound<InventoryDto>($"Inventory with id {id} is not found ");
            }
            inventory.ItemName = dto.ItemName;
            inventory.MinimumThreshold = dto.MinimumThreshold;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<InventoryDto>(inventory);
            return ResponseHandler.Success(result, "Inventory Updated succsessfully");
        }
    }
}
