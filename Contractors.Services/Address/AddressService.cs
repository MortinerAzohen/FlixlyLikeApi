using AutoMapper;
using Contractors.Data;
using Contractors.Data.DTOs;
using Contractors.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Services.Address
{
    public class AddressService : IAddressService
    {
        private readonly ContractorDbContext _db;
        private readonly UserManager<Contractor> _userManager;
        private readonly IMapper _mapper;
        public AddressService(ContractorDbContext db, UserManager<Contractor> userManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<BaseReturnModel<CompanyAddressDto>> AddAddressToCompany(string userId, CompanyAddressDto addrDto)
        {
            var baseModel = new BaseReturnModel<CompanyAddressDto>();
            var company = await _db.Companies.Include(c => c.Contractor)
                                             .Include(c => c.CompanyAddress)
                                             .FirstOrDefaultAsync(c => c.Id == addrDto.CompanyId);
            if(company == null)
            {
                baseModel.Model = null;
                baseModel.IsCorrect = false;
                baseModel.ErrorMessage = "Unable to find comapny with this Id";
                return baseModel;
            }
            else
            {

                if(company.Contractor.Id != userId)
                {
                    baseModel.Model = null;
                    baseModel.IsCorrect = false;
                    baseModel.ErrorMessage = "User is not the owner of company";
                    return baseModel;
                }

                company.CompanyAddress = _mapper.Map<Data.Models.Address>(addrDto);
                var success = await _db.SaveChangesAsync() > 0;
                if(success)
                {
                    baseModel.IsCorrect = true;
                    baseModel.Model = _mapper.Map<CompanyAddressDto>(company.CompanyAddress);
                    baseModel.Model.CompanyId = company.Id;
                    baseModel.Model.CompanyName = company.CompanyName;
                    return baseModel;
                }
                else
                {
                    baseModel.Model = null;
                    baseModel.IsCorrect = false;
                    baseModel.ErrorMessage = "Unable to add address to company";
                    return baseModel;
                }
            }
        }

        public async Task<BaseReturnModel<CompanyAddressDto>> UpdateCompanyAddress(string userId, CompanyAddressDto addrDto)
        {
            var baseModel = new BaseReturnModel<CompanyAddressDto>();
            var company = await _db.Companies.Include(c => c.Contractor)
                                             .Include(c => c.CompanyAddress)
                                             .FirstOrDefaultAsync(c => c.Id == addrDto.CompanyId);
            if (company == null)
            {
                baseModel.Model = null;
                baseModel.IsCorrect = false;
                baseModel.ErrorMessage = "Unable to find comapny with this Id";
                return baseModel;
            }
            else if (company.CompanyAddress == null)
            {
                baseModel.Model = null;
                baseModel.IsCorrect = false;
                baseModel.ErrorMessage = "Address not exist";
                return baseModel;
            }
            else
            {
                if (company.Contractor.Id != userId)
                {
                    baseModel.Model = null;
                    baseModel.IsCorrect = false;
                    baseModel.ErrorMessage = "User is not the owner of company";
                    return baseModel;
                }
                company.CompanyAddress = _mapper.Map<Data.Models.Address>(addrDto);
                var success = await _db.SaveChangesAsync() > 0;
                if (success)
                {
                    baseModel.IsCorrect = true;
                    baseModel.Model = _mapper.Map<CompanyAddressDto>(company.CompanyAddress);
                    baseModel.Model.CompanyId = company.Id;
                    baseModel.Model.CompanyName = company.CompanyName;
                    return baseModel;
                }
                else
                {
                    baseModel.Model = null;
                    baseModel.IsCorrect = false;
                    baseModel.ErrorMessage = "Unable to update address to company";
                    return baseModel;
                }
            }
        }
    }
}
