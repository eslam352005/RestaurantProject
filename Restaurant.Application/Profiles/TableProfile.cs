using AutoMapper;
using Restaurant.Application.DTOs.Table;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Profiles
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            CreateMap<Table, TableDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())).ReverseMap();
            CreateMap<CreateTableDto, Table>().ReverseMap();
            CreateMap<UpdateTableDto, Table>().ReverseMap();
        }
    }
}
