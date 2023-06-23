using FrogExhibitionBLL.Interfaces.IProvider;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FrogExhibitionBLL.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserEmail()
        {
            return _context.HttpContext.User.Claims
                       .First(i => i.Type == ClaimTypes.Email).Value;

        }
    }
}
