using Contractors.Data.DTOs;
using Contractors.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Services.Job
{
    public interface IJobService
    {
        public Task<BaseReturnModel<JobDto>> AddJobToCompany(JobDto jobDto, string userId);
        public Task<BaseReturnModel<JobDto>> UpdateJob(JobDto job, string userId);
        public Task<BaseReturnModel<List<JobDto>>> GetJobs();
        public Task<BaseReturnModel<List<JobDto>>> GetJobs(int idCompany);
        public Task<BaseReturnModel<List<JobDto>>> GetJobs(string categoryName);
        public Task<BaseReturnModel<List<JobDto>>> GetJobs(float xPrice, float yPrice);
        public Task<BaseReturnModel<List<JobDto>>> GetJobs(float xPrice, float yPrice, string categoryName);

    }
}
