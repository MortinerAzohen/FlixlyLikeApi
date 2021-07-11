using AutoMapper;
using Contractors.Data.DTOs;
using Contractors.Data.Models;
using Contractors.Services.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Contractors.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;


        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpGet("api/company/{id}")]
       public async Task<ActionResult> GetCompany(int id)
        {
            return Ok(await _companyService.GetCompany(id));
        }
        [HttpPost("api/company/create")]
        [Authorize(Roles = "CompanyOwner")]
        public async Task<ActionResult> AddCompany(CompanyDto companyDto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            return Ok(await _companyService.CreateNewCompany(companyDto, userId));
        }
        [HttpGet("api/companies")]
        public async Task<ActionResult> GetCompanies()
        {
            return Ok(await _companyService.GetAllCompanies());
        }
        [HttpPatch("api/company/activate/{id}")]
        [Authorize(Roles = "CompanyOwner")]
        public async Task<ActionResult> ActivateCompany(int id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            await _companyService.DeactivateCompany(id, userId);
            return Ok();
        }
        [HttpPatch("api/company/update")]
        [Authorize(Roles = "CompanyOwner")]
        public async Task<ActionResult> UpdateCompany(CompanyDto companyDto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            return Ok(await _companyService.UpdateCompanyInfo(companyDto, userId));
        }
        [HttpDelete("api/company/delete/{id}")]
        [Authorize(Roles = "CompanyOwner")]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            await _companyService.DeleteCompany(id, userId);
            return Ok();
        }
    }
}
