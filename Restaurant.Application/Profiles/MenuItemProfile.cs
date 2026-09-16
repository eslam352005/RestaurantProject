using AutoMapper;
using Restaurant.Application.DTOs.MenuItem;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<CreateMenuItemDto, MenuItem>().ReverseMap();
            CreateMap<UpdateMenuItemDto, MenuItem>().ReverseMap();
            CreateMap<MenuItem, MenuItemDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();
        }
    }
}
