using Contractors.Data.DTOs;
using Contractors.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contractors.Web.Controllers
{

    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService usrService)
        {
            _userService = usrService;
        }
        [HttpPost("api/user/register")]
        public async Task<ActionResult> Register(RegisterUserDto model)
        {
            var result = await _userService.Register(model);
            return Ok(result);
        }
        [HttpPost("api/user/login")]
        public async Task<ActionResult> Login(LoginUserDto model)
        {
            var result = await _userService.Login(model);
            return Ok(result);
        }
        [HttpPost("api/role/add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddRole(IdentityRole name)
        {
            var result = await _userService.AddRole(name.Name);
            return Ok(result);
        }
    }
}
