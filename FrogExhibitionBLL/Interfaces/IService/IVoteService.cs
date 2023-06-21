using FrogExhibitionBLL.DTO.VoteDtos;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IVoteService
    {
        Task<VoteDtoDetail> CreateVote(VoteDtoForCreate vote);
        Task DeleteVote(Guid id);
        Task<IEnumerable<VoteDtoDetail>> GetAllVotes();
        Task<VoteDtoDetail> GetVote(Guid id);
        Task UpdateVote(Guid id, VoteDtoForCreate vote);
    }
}