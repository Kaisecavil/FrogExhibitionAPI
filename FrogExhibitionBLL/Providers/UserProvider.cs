using FrogExhibitionBLL.Interfaces.IProvider;
using FrogExhibitionDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FrogExhibitionBLL.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProvider(IHttpContextAccessor context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }

        public string GetUserEmail()
        {
            return _context.HttpContext.User.Claims
                       .First(i => i.Type == ClaimTypes.Email).Value;

        }

        public async Task<string> GetUserIdAsync()
        {
            return (await _userManager.FindByEmailAsync(GetUserEmail())).Id;
        }
    }
}
