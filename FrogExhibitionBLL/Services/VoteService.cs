using AutoMapper;
using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.DTO.VoteDtos;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IProvider;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.VoteViewModels;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrogExhibitionBLL.Services
{
    public class VoteService : IVoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VoteService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;

        public VoteService(IUnitOfWork unitOfWork,
            ILogger<VoteService> logger,
            IMapper mapper,
            IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userProvider = userProvider;
        }

        public async Task<Guid> CreateVoteAsync(VoteDtoForCreate vote)
        {
            try
            {
                if (!await _unitOfWork.FrogOnExhibitions.EntityExistsAsync(vote.FrogOnExhibitionId))
                {
                    throw new NotFoundException("Can't find frog at exhibition");
                }

                var currentUserId = await _userProvider.GetUserIdAsync();
                var userVotesOnExhibitionCount = _unitOfWork.Votes
                    .GetAllQueryable()
                    .Where(v => v.ApplicationUserId == currentUserId
                    && v.FrogOnExhibitionId == vote.FrogOnExhibitionId)
                    .Count();

                if (userVotesOnExhibitionCount >= ConstraintValueConstants.MaxQuantityOfVotesPerUserAtExhibition)
                {
                    throw new DbUpdateException("This user has cast all of his available votes on this exebiton");
                }

                try
                {
                    var mappedVote = _mapper.Map<Vote>(vote);
                    mappedVote.ApplicationUserId = currentUserId;
                    await _unitOfWork.Votes.CreateAsync(mappedVote);
                    _logger.LogInformation("Vote Created");
                    await _unitOfWork.SaveAsync();
                    return mappedVote.Id;
                }
                catch (DbUpdateException ex)
                {
                    throw new DbUpdateException("You can't vote for the same frog on exebition twice");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, vote);
                throw;
            };
        }

        public async Task<IEnumerable<VoteDetailViewModel>> GetAllVotesAsync()
        {
            var result = await _unitOfWork.Votes.GetAllAsync(true);
            return _mapper.Map<IEnumerable<VoteDetailViewModel>>(result);
        }

        public async Task<VoteDetailViewModel> GetVoteAsync(Guid id)
        {
            var vote = await _unitOfWork.Votes.GetAsync(id, true);
            if (vote == null)
            {
                throw new NotFoundException("Entity not found");
            }

            return _mapper.Map<VoteDetailViewModel>(vote);
        }

        public async Task UpdateVoteAsync(VoteDtoForUpdate vote)
        {
            try
            {
                if (!(await _unitOfWork.Votes.EntityExistsAsync(vote.Id)))
                {
                    throw new NotFoundException("Entity not found");
                }

                if (!await _unitOfWork.FrogOnExhibitions.EntityExistsAsync(vote.FrogOnExhibitionId))
                {
                    throw new NotFoundException("Frog on exhibition not found");
                }

                var currentUserId = await _userProvider.GetUserIdAsync();
                var userVotesOnExhibitionCount = _unitOfWork.Votes.GetAllQueryable()
                    .Where(v => v.ApplicationUserId == currentUserId
                    && v.FrogOnExhibitionId == vote.FrogOnExhibitionId)
                    .Count();

                if (userVotesOnExhibitionCount >= ConstraintValueConstants.MaxQuantityOfVotesPerUserAtExhibition)
                {
                    throw new DbUpdateException("This user has cast all of his available votes on this exebiton");
                }

                var mappedVote = _mapper.Map<Vote>(vote);
                mappedVote.ApplicationUserId = currentUserId;
                await _unitOfWork.Votes.UpdateAsync(mappedVote);
                await _unitOfWork.SaveAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _unitOfWork.Votes.EntityExistsAsync(vote.Id))
                {
                    throw new NotFoundException("Entity not found due to possible concurrency");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, vote);
                throw;
            };
        }

        public async Task DeleteVoteAsync(Guid id)
        {

            var currentUserId = await _userProvider.GetUserIdAsync();
            var vote = await _unitOfWork.Votes.GetAsync(id);
            if (vote == null)
            {
                throw new NotFoundException("Entity not found");
            }

            if (vote.ApplicationUserId != currentUserId)
            {
                throw new ForbidException("You can't delete other users votes");
            }

            await _unitOfWork.Votes.DeleteAsync(vote.Id);
            await _unitOfWork.SaveAsync();

        }
    }
}
