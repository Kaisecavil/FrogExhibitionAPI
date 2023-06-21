using FrogExhibitionBLL.DTO.FrogDTOs;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IFrogService
    {
        public Task<IEnumerable<FrogDtoGeneral>> GetAllFrogs();
        public Task<IEnumerable<FrogDtoGeneral>> GetAllFrogs(string sortParams);
        public Task<FrogDtoDetail> GetFrog(Guid id);
        public Task<FrogDtoDetail> CreateFrog(FrogDtoForCreate frog);
        public Task DeleteFrog(Guid id);
        //?public Task UpdateFrog(Guid id, FrogDtoForUpdate frog);
        public Task UpdateFrog(FrogDtoForUpdate frog);
    }
}
