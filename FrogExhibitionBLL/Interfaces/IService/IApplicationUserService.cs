using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IApplicationUserService
    {
        Task DeleteApplicationUserAsync(Guid id);
        Task<IEnumerable<ApplicationUserDetailViewModel>> GetAllApplicationUsersAsync();
        Task<ApplicationUserDetailViewModel> GetApplicationUserAsync(Guid id);
        Task<FileContentResult> GetUserStatisticsAsync(Guid id);
        Task UpdateApplicationUserAsync(ApplicationUserDtoForUpdate applicationUser);
    }
}