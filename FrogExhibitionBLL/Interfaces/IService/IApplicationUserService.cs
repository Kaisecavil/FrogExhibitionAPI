using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;
using FrogExhibitionBLL.ViewModels.VoteViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IApplicationUserService
    {
        Task DeleteApplicationUserAsync(Guid id);
        Task<IEnumerable<ApplicationUserDetailViewModel>> GetAllApplicationUsersAsync();
        Task<ApplicationUserDetailViewModel> GetApplicationUserAsync(Guid id);
        Task<bool> GetUserLastVotesOnExhibitionsAsync(Guid id, int quantityOfLastExhibitions);
        Task<ApplicationUserReportViewModel> GetUserStatisticsAsync(Guid id);
        Task<FileContentResult> GetUserStatisticsReportAsync(Guid id);
        Task UpdateApplicationUserAsync(ApplicationUserDtoForUpdate applicationUser);
    }
}