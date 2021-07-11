using Contractors.Data.DTOs;
using Contractors.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Services.User
{
    public interface IUserService
    {
        public Task<bool> Register(RegisterUserDto model);
        public Task<AuthenticationModel> Login(LoginUserDto model);
        public Task<bool> AddRole(string roleName);
    }
}
