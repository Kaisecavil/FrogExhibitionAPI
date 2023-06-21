using FrogExhibitionBLL.DTO.VoteDtos;
using FrogExhibitionBLL.ViewModels.VoteViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IVoteService
    {
        Task<VoteDetailViewModel> CreateVote(VoteDtoForCreate vote);
        Task DeleteVote(Guid id);
        Task<IEnumerable<VoteDetailViewModel>> GetAllVotes();
        Task<VoteDetailViewModel> GetVote(Guid id);
        Task UpdateVote(Guid id, VoteDtoForCreate vote);
    }
}