using AutoMapper;
using Restaurant.Application.DTOs.Branch;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Profiles
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<BranchDto, Branch>().ReverseMap();
            CreateMap<CreateBranchDto, Branch>().ReverseMap();
            CreateMap<UpdateBranchDto, Branch>().ReverseMap();
        }
    }
}
