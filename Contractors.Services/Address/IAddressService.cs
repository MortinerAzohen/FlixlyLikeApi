using Contractors.Data.DTOs;
using Contractors.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Services.Address
{
    public interface IAddressService
    {
        public Task<BaseReturnModel<CompanyAddressDto>> AddAddressToCompany(string userId, CompanyAddressDto address);
        public Task<BaseReturnModel<CompanyAddressDto>> UpdateCompanyAddress(string userId, CompanyAddressDto address);

    }
}
