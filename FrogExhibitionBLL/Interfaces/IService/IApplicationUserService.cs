using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IApplicationUserService
    {
        Task DeleteApplicationUser(Guid id);
        Task<IEnumerable<ApplicationUserDetailViewModel>> GetAllApplicationUsers();
        Task<ApplicationUserDetailViewModel> GetApplicationUser(Guid id);
        Task UpdateApplicationUser(Guid id, ApplicationUserDtoForUpdate applicationUser);
    }
}