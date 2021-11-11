using Contractors.Data.DTOs;
using Contractors.Services.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Contractors.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("api/jobs")]
        public async Task<ActionResult> GetJobs()
        {
            return Ok(await _jobService.GetJobs());
        }
        [HttpGet("api/jobs/jobsByCompany/{id}")]
        public async Task<ActionResult> GetJobs(int id)
        {
            return Ok(await _jobService.GetJobs(id));
        }
        [HttpGet("api/jobs/jobsByCategory{categoryName}")]
        public async Task<ActionResult> GetJobs(string categoryName)
        {
            return Ok(await _jobService.GetJobs(categoryName));
        }
        [HttpGet("api/jobs/jobsByPrize/{xPrize}/{yPrize}")]
        public async Task<ActionResult> GetJobs(float xPrize, float yPrize)
        {

            return Ok(await _jobService.GetJobs(xPrize, yPrize));
        }
        [HttpGet("api/jobs/jobsByPrizeAndCategory/{xPrize}/{yPrize}/{categoryName}")]
        public async Task<ActionResult> GetJobs(float xPrize, float yPrize, string categoryName)
        {
            return Ok(await _jobService.GetJobs(xPrize, yPrize, categoryName));
        }
        [HttpPatch("api/jobs/update/{id}")]
        [Authorize(Roles = "CompanyOwner")]
        public async Task<ActionResult> UpdateJob(JobDto jobDto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            return Ok(await _jobService.UpdateJob(jobDto, userId));
        }
        [HttpPost("api/jobs/addToCompany/{id}")]
        [Authorize(Roles = "CompanyOwner")]
        public async Task<ActionResult> AddJob(int id, JobDto jobDto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
            return Ok(await _jobService.AddJobToCompany(jobDto, userId));
        }
    }
}
