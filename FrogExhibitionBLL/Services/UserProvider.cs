using FrogExhibitionBLL.Interfaces.IService;
using System.Security.Claims;

namespace FrogExhibitionBLL.Services
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
