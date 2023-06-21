using FrogExhibitionBLL.Interfaces.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FrogExhibitionBLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }

        public string GenerateTokenString(LoginUser user, IEnumerable<string> roles)
        {
           
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                roles.Contains("Admin")? new Claim(ClaimTypes.Role, "Admin") : new Claim(ClaimTypes.Role, "User")
            };
            
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

        public async Task<bool> Login(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser == null)
            {
                return false;
            }
            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        public async Task<bool> RegisterUser(LoginUser user)
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
