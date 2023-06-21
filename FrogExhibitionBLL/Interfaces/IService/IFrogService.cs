using FrogExhibitionBLL.DTO.FrogDTOs;
using FrogExhibitionBLL.ViewModels.FrogViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IFrogService
    {
        public Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogs();
        public Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogs(string sortParams);
        public Task<FrogDetailViewModel> GetFrog(Guid id);
        public Task<FrogDetailViewModel> CreateFrog(FrogDtoForCreate frog);
        public Task DeleteFrog(Guid id);
        //?public Task UpdateFrog(Guid id, FrogDtoForUpdate frog);
        public Task UpdateFrog(FrogDtoForUpdate frog);
    }
}
