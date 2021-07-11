using System.Collections.Generic;
using System.Threading.Tasks;
using Contractors.Data.DTOs;
using Contractors.Data.Models;


namespace Contractors.Services.Company
{
    public interface ICompanyService
    {
        public Task<BaseReturnModel<List<CompanyDto>>> GetAllCompanies();

        public Task<BaseReturnModel<CompanyDto>> GetCompany(int id);
        public Task<BaseReturnModel<CompanyDto>> UpdateCompanyInfo(CompanyDto companyDto, string userId);
        public Task<BaseReturnModel<CompanyDto>> CreateNewCompany(CompanyDto companyDto, string userId);
        public Task<bool> DeleteCompany(int id, string userId);
        public Task<bool> DeactivateCompany(int id, string userId);     
    }
}
