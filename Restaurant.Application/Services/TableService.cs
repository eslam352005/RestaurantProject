using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.DTOs.Table;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Services
{
    public class TableService(IApplicationDbcontext _context, IMapper _mapper) : ITableService
    {
        public async Task<ResponseDto<TableDto>> CreateTableAsync(CreateTableDto tableDto)
        {
           var table = _mapper.Map<Table>(tableDto);
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
            var createdTableDto = _mapper.Map<TableDto>(table);
            return ResponseHandler.Success(createdTableDto, "Table created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteTableAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return ResponseHandler.NotFound<bool>("Table not found");
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return ResponseHandler.Success(true, "Table deleted successfully");
        }

        public async Task<ResponseDto<IEnumerable<TableDto>>> GetAllTablesAsync(int? branchId)
        {
            var tables = await _context.Tables
                .Where(t => !branchId.HasValue || t.BranchId == branchId.Value)
                .ToListAsync();
            var tableDtos = _mapper.Map<IEnumerable<TableDto>>(tables);
            return ResponseHandler.Success(tableDtos, "Tables retrieved successfully");
        }

        public async Task<ResponseDto<TableDto>> GetTableByIdAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            var tableDto = _mapper.Map<TableDto>(table);
            return ResponseHandler.Success(tableDto, "Table retrieved successfully");
        }

        public async Task<ResponseDto<TableDto>> UpdateTableAsync(int id, UpdateTableDto tableDto)
        {
           var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return ResponseHandler.NotFound<TableDto>("Table not found");
            }

            table.TableNumber = tableDto.TableNumber;
            table.Capacity = tableDto.Capacity;

            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
            var updatedTableDto = _mapper.Map<TableDto>(table);
            return ResponseHandler.Success(updatedTableDto, "Table updated successfully");
        }

        public async Task<ResponseDto<TableDto>> UpdateTableStatusAsync(int id, UpdateTableStatusDto statusDto)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return ResponseHandler.NotFound<TableDto>("Table not found");
            }
            table.Status = statusDto.Status;
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
            var tableDto = _mapper.Map<TableDto>(table);
            return ResponseHandler.Success(tableDto, "Table status updated successfully");
        }
    }
}
