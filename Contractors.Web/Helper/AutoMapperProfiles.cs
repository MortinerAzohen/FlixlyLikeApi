using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contractors.Data.DTOs;
using Contractors.Data.Models;

namespace Contractors.Web.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CompanyDto, Company>()
                .ForPath(dest => dest.Contractor.UserName, opt => opt.MapFrom(src => src.CompanyContractorName));
            CreateMap<Company, CompanyDto>()
                .ForPath(dest => dest.CompanyContractorName, opt => opt.MapFrom(src => src.Contractor.UserName));
            CreateMap<Job, JobDto>()
                .ForPath(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company.Id))
                .ForPath(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName))
                .ForPath(dest => dest.JobCategoryId, opt => opt.MapFrom(src => src.JobCategory.Id))
                .ForPath(dest => dest.JobCategoryName, opt => opt.MapFrom(src => src.JobCategory.CategoryName));
            CreateMap<JobDto, Job>()
                .ForPath(dest => dest.Company.Id, opt => opt.MapFrom(src => src.CompanyId))
                .ForPath(dest => dest.Company.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForPath(dest => dest.JobCategory.Id, opt => opt.MapFrom(src => src.JobCategoryId))
                .ForPath(dest => dest.JobCategory.CategoryName, opt => opt.MapFrom(src => src.JobCategoryName));
            CreateMap<Address, CompanyAddressDto>();
            CreateMap<CompanyAddressDto, Address>();

        }
    }
}
