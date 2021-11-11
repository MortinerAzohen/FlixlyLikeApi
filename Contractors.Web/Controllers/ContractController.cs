using Contractors.Data.DTOs;
using Contractors.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contractors.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;


        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet("api/offers/{id}")]
        public async Task<ActionResult> GetOffer(int id)
        {
            return Ok(await _contractService.GetOffer(id));
        }
        [HttpGet("api/offers")]
        public async Task<ActionResult> GetMyOffers()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            return Ok(await _contractService.GetMyOffers(userId));
        }
        [HttpGet("api/contracts")]
        public async Task<ActionResult> GetMyContracts()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            return Ok(await _contractService.GetMyContracts(userId));
        }
        [HttpGet("api/contracts/history/{id}")]
        public async Task<ActionResult> GetContractHistory(int id)
        {
            return Ok(await _contractService.GetHistoryOfContract(id));
        }
        [HttpGet("api/contracts/{id}")]
        public async Task<ActionResult> GetContract(int id)
        {
            return Ok(await _contractService.GetContract(id));
        }
        [HttpPost("api/offer/create")]
        public async Task<ActionResult> AddOffer(OfferDto offerDto)
        {
            return Ok(await _contractService.CreateOffer(offerDto));
        }
        [HttpPost("api/offer/{id}/accept")]
        public async Task<ActionResult> AcceptCompany(int id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            return Ok(await _contractService.AcceptOffer(userId,id));
        }
        [HttpPost("api/offer/reply")]
        public async Task<ActionResult> ReplyOffer(OfferDto offerDto)
        {
            return Ok(await _contractService.CreateReplayForOffer(offerDto));
        }

    }
}
