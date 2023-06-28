using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IAuthService
    {
        string GenerateTokenString(ApplicationUserDtoForLogin user, IEnumerable<string> roles);
        Task<string> LoginAsync(ApplicationUserDtoForLogin user);
        Task<bool> RegisterUserAsync(ApplicationUserDtoForLogin user);
    }
}