using Contractors.Data.DTOs;
using Contractors.Data.Models;
using Contractors.Data.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Contractors.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Contractor> _userManager;
        private readonly SignInManager<Contractor> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public UserService(UserManager<Contractor> userManager,
                           SignInManager<Contractor> signInManager,
                           RoleManager<IdentityRole> roleManager,
                           IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthenticationModel> Login(LoginUserDto model)
        {
            var authModel = new AuthenticationModel();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = $"No Accounts Registered with {model.UserName}.";
                return authModel;
            }
            var isPassCorrect = await _userManager.CheckPasswordAsync(user,model.Password);
            if(isPassCorrect)
            {
                authModel.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = await CreateToken(user);
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.Email = user.Email;
                authModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authModel.Roles = rolesList.ToList();
                return authModel;
            }
            authModel.IsAuthenticated = false;
            authModel.Message = $"Incorrect Credentials for user {user.UserName}.";
            return authModel;
        }
        private async Task<JwtSecurityToken> CreateToken(Contractor user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
        public async Task<bool> Register(RegisterUserDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null)
            {
                return false;
            }

            user = new Contractor
            {
                UserName = model.UserName,
                Email = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                if (model.IsModerator)
                {
                    await _userManager.AddToRoleAsync(user, "Moderator");
                }
                if (model.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                if (model.IsCompanyOwner)
                {
                    await _userManager.AddToRoleAsync(user, "CompanyOwner");
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> AddRole(string roleName)
        {
            var role = new IdentityRole
            {
                Name = roleName
            };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

    }
}
