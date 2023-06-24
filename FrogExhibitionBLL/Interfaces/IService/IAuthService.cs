using FrogExhibitionDAL.Models;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IAuthService
    {
        string GenerateTokenString(LoginUser user, IEnumerable<string> roles);
        Task<string> LoginAsync(LoginUser user);
        Task<bool> RegisterUserAsync(LoginUser user);
    }
}