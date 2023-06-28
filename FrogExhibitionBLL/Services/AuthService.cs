using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FrogExhibitionBLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public string GenerateTokenString(ApplicationUserDtoForLogin user, IEnumerable<string> roles)
        {

            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                roles.Contains(RoleConstants.AdminRole)?
                new Claim(ClaimTypes.Role,  RoleConstants.AdminRole) : new Claim("isAdmin","No"),
                new Claim(ClaimTypes.Role,  RoleConstants.UserRole)
            };
            claims.Append(new Claim(ClaimTypes.Role, RoleConstants.UserRole));
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            SigningCredentials signingCred = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha512Signature);
            SecurityToken securityToken = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials:signingCred);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }

        public async Task<string> LoginAsync(ApplicationUserDtoForLogin user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            if (appUser == null)
            {
                throw new BadRequestException("Wrong username or password");
            }
            if(await _userManager.CheckPasswordAsync(appUser, user.Password))
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                return GenerateTokenString(user, roles);
            }
            throw new BadRequestException("Wrong username or password");
        }

        

        public async Task<bool> RegisterUserAsync(ApplicationUserDtoForLogin user)
        {
            var existingUser = _userManager.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser == null)
            {
                var identityUser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email
                };

                var result = await _userManager.CreateAsync(identityUser, user.Password);
                return result.Succeeded;
            }
            else
            {
                throw new DbUpdateException("User with the same email alredy exists");
            }
        }

    }
}
