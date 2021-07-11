using AutoMapper;
using Contractors.Data;
using Contractors.Data.DTOs;
using Contractors.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Services.Job
{
    public class JobService : IJobService
    {
        private readonly ContractorDbContext _db;
        private readonly IMapper _mapper;

        public JobService(ContractorDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<BaseReturnModel<JobDto>> AddJobToCompany(JobDto jobDto, string userId)
        {
            var baseModel = new BaseReturnModel<JobDto>();
            var company = await _db.Companies.Include(c => c.Contractor).FirstOrDefaultAsync(c => c.Id == jobDto.CompanyId);
            if (company == null)
            {
                baseModel.ErrorMessage = "Company not exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else if(company.Contractor.Id != userId)
            {
                baseModel.ErrorMessage = "User is not the owner of the company";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                var job = _mapper.Map<Data.Models.Job>(jobDto);
                job.Company = company;
                _db.Add(job);
                var success = await _db.SaveChangesAsync() > 0;
                if (success)
                {
                    baseModel.IsCorrect = true;
                    baseModel.Model = _mapper.Map<JobDto>(job);
                    return baseModel;
                }
                else
                {
                    baseModel.ErrorMessage = "Unable to add job to database";
                    baseModel.IsCorrect = false;
                    baseModel.Model = null;
                    return baseModel;
                }
            }
        }


        public async Task<BaseReturnModel<List<JobDto>>> GetJobs()
        {
            var baseModel = new BaseReturnModel<List<JobDto>>();
            var jobs = await _db.Jobs
                            .Include(j => j.JobCategory)
                            .Include(j => j.Company)
                            .OrderBy(j => j.JobCategory)
                            .OrderBy(j => j.JobName)
                            .ToListAsync();
            foreach(Data.Models.Job job in jobs)
            {
                baseModel.Model.Add(_mapper.Map<JobDto>(job));
            }
            baseModel.IsCorrect = true;
            return baseModel;
        }

        public async Task<BaseReturnModel<List<JobDto>>> GetJobs(int idCompany)
        {
            var baseModel = new BaseReturnModel<List<JobDto>>();
            var jobs = await _db.Jobs
                            .Where(j => j.Company.Id == idCompany)
                            .Include(j => j.JobCategory)
                            .Include(j => j.Company)
                            .OrderBy(j => j.JobCategory)
                            .OrderBy(j => j.JobName)
                            .ToListAsync();
            foreach (Data.Models.Job job in jobs)
            {
                baseModel.Model.Add(_mapper.Map<JobDto>(job));
            }
            baseModel.IsCorrect = true;
            return baseModel;
        }

        public async Task<BaseReturnModel<List<JobDto>>> GetJobs(string categoryName)
        {
            var baseModel = new BaseReturnModel<List<JobDto>>();
            var jobs = await _db.Jobs
                            .Where(j => j.JobCategory.CategoryName == categoryName)
                            .Include(j => j.JobCategory)
                            .Include(j => j.Company)
                            .OrderBy(j => j.JobName)
                            .ToListAsync();
            foreach (Data.Models.Job job in jobs)
            {
                baseModel.Model.Add(_mapper.Map<JobDto>(job));
            }
            baseModel.IsCorrect = true;
            return baseModel;
        }

        public async Task<BaseReturnModel<List<JobDto>>> GetJobs(float xPrice, float yPrice)
        {
            var baseModel = new BaseReturnModel<List<JobDto>>();
            var jobs = await _db.Jobs
                            .Where(j => j.JobPricing >= xPrice && j.JobPricing < yPrice)
                            .Include(j => j.JobCategory)
                            .Include(j => j.Company)
                            .OrderBy(j => j.JobName)
                            .ToListAsync();
            foreach (Data.Models.Job job in jobs)
            {
                baseModel.Model.Add(_mapper.Map<JobDto>(job));
            }
            baseModel.IsCorrect = true;
            return baseModel;
        }

        public async Task<BaseReturnModel<List<JobDto>>> GetJobs(float xPrice, float yPrice, string categoryName)
        {
            var baseModel = new BaseReturnModel<List<JobDto>>();
            var jobs = await _db.Jobs
                            .Where(j => j.JobPricing >= xPrice && j.JobPricing < yPrice && j.JobCategory.CategoryName == categoryName)
                            .Include(j => j.JobCategory)
                            .Include(j => j.Company)
                            .OrderBy(j => j.JobName)
                            .ToListAsync();
            foreach (Data.Models.Job job in jobs)
            {
                baseModel.Model.Add(_mapper.Map<JobDto>(job));
            }
            baseModel.IsCorrect = true;
            return baseModel;
        }

        public async Task<BaseReturnModel<JobDto>> UpdateJob(JobDto jobDto, string userId)
        {
            var baseModel = new BaseReturnModel<JobDto>();
            var company = await _db.Companies
                                   .Include(c => c.Contractor)
                                   .Include(c => c.CompanyJobs)
                                   .ThenInclude(j => j.JobCategory)
                                   .SingleOrDefaultAsync(c => c.Id == jobDto.CompanyId);
            if(company == null)
            {
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                baseModel.ErrorMessage = "Company not exist";
                return baseModel;
            }
            else if(company.Contractor.Id != userId)
            {
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                baseModel.ErrorMessage = "Company is not owned by user";
                return baseModel;
            }
            else
            {
                var jobForUpdate = company.CompanyJobs.FirstOrDefault(j=>j.Id == jobDto.Id);
                if(jobForUpdate == null )
                {
                    baseModel.IsCorrect = false;
                    baseModel.Model = null;
                    baseModel.ErrorMessage = "Unable to find job";
                    return baseModel;
                }
                else
                {
                    jobForUpdate.JobAbout = jobDto.JobAbout;
                    jobForUpdate.JobDuration = jobDto.JobDuration;
                    jobForUpdate.JobName = jobDto.JobName;
                    jobForUpdate.JobPricing = jobDto.JobPricing;
                    jobForUpdate.JobPricingCurrency = jobDto.JobPricingCurrency;

                    var success = await _db.SaveChangesAsync() > 0;
                    if (success)
                    {
                        baseModel.IsCorrect = true;
                        baseModel.Model = _mapper.Map<JobDto>(jobForUpdate);
                        return baseModel;
                    }
                    else
                    {
                        baseModel.IsCorrect = false;
                        baseModel.Model = null;
                        baseModel.ErrorMessage = "Unable to update job";
                        return baseModel;
                    }

                }
            }
        }
    }
}
