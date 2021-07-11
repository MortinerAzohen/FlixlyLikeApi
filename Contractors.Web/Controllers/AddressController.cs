using Contractors.Data.DTOs;
using Contractors.Data.Models;
using Contractors.Services.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contractors.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost("api/company/{id}/address/create")]
        [Authorize(Roles = "CompanyOwner")]

        public async Task<ActionResult> AddCompanyAddress(CompanyAddressDto address)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            return Ok(await _addressService.AddAddressToCompany(userId, address));
        }
        [HttpPatch("api/company/{id}/address/update")]
        [Authorize(Roles = "CompanyOwner")]
        public async Task<ActionResult> UpdateCompanyAddress(CompanyAddressDto address)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            var addr = await _addressService.UpdateCompanyAddress(id, address);
            return Ok(addr);
        }

    }
}
