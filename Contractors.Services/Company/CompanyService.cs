using AutoMapper;
using Contractors.Data;
using Contractors.Data.DTOs;
using Contractors.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contractors.Services.Company
{
    public class CompanyService : ICompanyService
    {
        private readonly ContractorDbContext _db;
        private readonly UserManager<Contractor> _userManager;
        private readonly IMapper _mapper;

        public CompanyService(ContractorDbContext db, UserManager<Contractor> userManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;

        }
        public async Task<BaseReturnModel<CompanyDto>> CreateNewCompany(CompanyDto companyDto, string userId)
        {
            var baseModel = new BaseReturnModel<CompanyDto>();
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                baseModel.ErrorMessage = "User with this Id doesnt exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }

            var company = _mapper.Map<Data.Models.Company>(companyDto);
            company.UpatedOn = DateTime.Now;
            company.CreatedOn = DateTime.Now;
            company.Contractor = user;

            _db.Add(company);
            var success = await _db.SaveChangesAsync() > 0;
            if (success)
            {
                baseModel.IsCorrect = true;
                baseModel.Model = _mapper.Map<CompanyDto>(company);
                return baseModel;
            }
            else
            {
                baseModel.ErrorMessage = "Unable to add company to database";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
        }

        public async Task<bool> DeactivateCompany(int id, string userId)
        {

            var company = await _db.Companies.Where(c => c.Contractor.Id == userId).FirstOrDefaultAsync();

            if (company == null)
            {
                throw new Exception("Unable to find selected company");
            }
            if (company.IsActive == false)
            {
                company.IsActive = true;
            }
            else if (company.IsActive == true)
            {
                company.IsActive = false;
            }
            else
            {
                company.IsActive = false;
            }
            var success = await _db.SaveChangesAsync() > 0;
            if (success) return true;
            else
            {
                throw new Exception("Unable to save changes");
            }
        }

        public async Task<bool> DeleteCompany(int id, string userId)
        {
            var company = await _db.Companies.Where(c => c.Contractor.Id == userId).FirstOrDefaultAsync();
            if (company == null)
            {
                throw new Exception("Unable to find selected company");
            }
            _db.Companies.Remove(company);
            var success = await _db.SaveChangesAsync() > 0;
            if (success) return true;
            else
            {
                throw new Exception("Unable to save changes");
            }
        }
        public async Task<BaseReturnModel<List<CompanyDto>>> GetAllCompanies()
        {
            var baseModel = new BaseReturnModel<List<CompanyDto>>();
            var companies = await _db.Companies.Include(c => c.CompanyAddress)
                                      .Include(c => c.Contractor)
                                      .OrderBy(c => c.CompanyName)
                                      .ToListAsync();
            if(companies.Any() == false)
            {
                baseModel.ErrorMessage = "No companies in database";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                baseModel.IsCorrect = true;
                baseModel.Model = _mapper.Map<List<CompanyDto>>(companies);
                return baseModel;
            }
        }

        public async Task<BaseReturnModel<CompanyDto>> GetCompany(int id)
        {
            var baseModel = new BaseReturnModel<CompanyDto>();
            var company = await _db.Companies.Include(c => c.Contractor)
                                             .Include(c => c.CompanyAddress)
                                             .FirstOrDefaultAsync(c => c.Id == id);
            if(company == null)
            {
                baseModel.ErrorMessage = "Company with this Id doesnt exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                baseModel.IsCorrect = true;
                baseModel.Model = _mapper.Map<CompanyDto>(company);
                return baseModel;
            }
        }

        public async Task<BaseReturnModel<CompanyDto>> UpdateCompanyInfo(CompanyDto companyDto, string userId)
        {
            var baseModel = new BaseReturnModel<CompanyDto>();
            var company = await _db.Companies.Include(c => c.Contractor).FirstOrDefaultAsync(c => c.Id == companyDto.Id);
            if (company == null)
            {
                baseModel.ErrorMessage = "Company doesnt exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else if (company.Contractor.Id != userId)
            {
                baseModel.ErrorMessage = "User is not owner of this company";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                company.CompanyAbout = companyDto.CompanyAbout;
                company.CompanyAddress = companyDto.CompanyAddress;
                company.CompanyName = company.CompanyName;

                var result = await _db.SaveChangesAsync() > 0;

                if (result == false)
                {
                    baseModel.ErrorMessage = "Unable to save changes";
                    baseModel.IsCorrect = false;
                    baseModel.Model = null;
                    return baseModel;
                }
                else
                {
                    baseModel.IsCorrect = true;
                    baseModel.Model = _mapper.Map<CompanyDto>(company);
                    return baseModel;
                }
            }
        }
    }
}
