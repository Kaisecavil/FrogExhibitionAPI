using FrogExhibitionBLL.DTO.VoteDtos;
using FrogExhibitionBLL.ViewModels.VoteViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IVoteService
    {
        Task<Guid> CreateVoteAsync(VoteDtoForCreate vote);
        Task DeleteVoteAsync(Guid id);
        Task<IEnumerable<VoteDetailViewModel>> GetAllVotesAsync();
        Task<VoteDetailViewModel> GetVoteAsync(Guid id);
        Task UpdateVoteAsync(VoteDtoForUpdate vote);
    }
}