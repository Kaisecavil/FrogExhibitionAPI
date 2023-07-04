using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.ViewModels.ExhibitionViewModels;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IExhibitionService
    {
        Task<Guid> CreateExhibitionAsync(ExhibitionDtoForCreate exebition);
        Task DeleteExhibitionAsync(Guid id);
        Task<IEnumerable<ExhibitionGeneralViewModel>> GetAllExhibitionsAsync();
        Task<ExhibitionDetailViewModel> GetExhibitionAsync(Guid id);
        Task UpdateExhibitionAsync(ExhibitionDtoForUpdate exebition);
        Task<IEnumerable<FrogRatingViewModel>> GetRatingAsync(Guid id);
        Task<IEnumerable<ExhibitionGeneralViewModel>> GetAllExhibitionsAsync(string sortParams);
        Task<IEnumerable<FrogRatingViewModel>> GetBestFrogsHistoryAsync();
        Task<FileContentResult> GetExhibitionStatisticsReportAsync(Guid id);
        IEnumerable<FrogRatingViewModel> GetRating(Guid id);
        Task<int> GetFrogsPlaceOnExhibitionAsync(Guid frogid, Guid exhibitionId);
        Task<ExhibitionReportViewModel> GetExhibitionStatisticsAsync(Guid id);
    }
}