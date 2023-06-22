using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IApplicationUserService
    {
        Task DeleteApplicationUserAsync(Guid id);
        Task<IEnumerable<ApplicationUserDetailViewModel>> GetAllApplicationUsersAsync();
        Task<ApplicationUserDetailViewModel> GetApplicationUserAsync(Guid id);
        Task UpdateApplicationUserAsync(Guid id, ApplicationUserDtoForUpdate applicationUser);
    }
}