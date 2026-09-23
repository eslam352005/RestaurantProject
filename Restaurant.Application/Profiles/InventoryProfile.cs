using AutoMapper;
using Restaurant.Application.DTOs.Inventory;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryDto>()
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.QuantityAvailable<=src.MinimumThreshold)).ReverseMap();
            CreateMap<Inventory, CreateInventoryDto>().ReverseMap();

        }
    }
}
