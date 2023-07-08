using AutoMapper;
using FrogExhibitionBLL.DTO.CommentsDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IProvider;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.CommentViewModels;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrogExhibitionBLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CommentService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;

        public CommentService(IUnitOfWork unitOfWork,
            ILogger<CommentService> logger,
            IMapper mapper,
            IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userProvider = userProvider;
        }

        public async Task<Guid> CreateCommentAsync(CommentDtoForCreate comment)
        {
            try
            {
                if (await _unitOfWork.FrogOnExhibitions.EntityExistsAsync(comment.FrogOnExhibitionId))
                {
                    var currentUserId = await _userProvider.GetUserIdAsync();
                    var mappedComment = _mapper.Map<Comment>(comment);
                    mappedComment.ApplicationUserId = currentUserId;
                    mappedComment.CreationDate = DateTime.Now;
                    await _unitOfWork.Comments.CreateAsync(mappedComment);
                    await _unitOfWork.SaveAsync();
                    return mappedComment.Id;
                }
                else
                {
                    throw new NotFoundException("Can't find this frog at exhibition");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, comment);
                throw;
            };
        }

        public async Task<IEnumerable<CommentGeneralViewModel>> GetAllCommentsAsync()
        {
            var result = await _unitOfWork.Comments.GetAllAsync(true);
            return _mapper.Map<IEnumerable<CommentGeneralViewModel>>(result);
        }

        public async Task<CommentGeneralViewModel> GetCommentAsync(Guid id)
        {
            var comment = await _unitOfWork.Comments.GetAsync(id, true);
            if (comment == null)
            {
                throw new NotFoundException("Entity not found");
            }
            return _mapper.Map<CommentGeneralViewModel>(comment);
        }

        public async Task UpdateCommentAsync(CommentDtoForUpdate comment)
        {
            try
            {

                if (await _unitOfWork.Comments.EntityExistsAsync(comment.Id))
                {
                    var currentUserId = await _userProvider.GetUserIdAsync();
                    var commentToUpdate = await _unitOfWork.Comments.GetAsync(comment.Id);
                    if (commentToUpdate.ApplicationUserId == currentUserId)
                    {
                        var mappedComment = _mapper.Map<Comment>(comment);
                        mappedComment.ApplicationUserId = currentUserId;
                        await _unitOfWork.Comments.UpdateAsync(mappedComment);
                        await _unitOfWork.SaveAsync();
                    }
                    else
                    {
                        throw new BadRequestException("You can't update comments of other users");
                    }

                }
                else
                {
                    throw new NotFoundException("Entity not found");
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _unitOfWork.Comments.EntityExistsAsync(comment.Id))
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
                _logger.LogError(ex.Message, comment);
                throw;
            };
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            var currentUserId = await _userProvider.GetUserIdAsync();
            var comment = await _unitOfWork.Comments.GetAsync(id);
            if (comment == null)
            {
                throw new NotFoundException("Entity not found");
            }
            if(comment.ApplicationUserId == currentUserId)
            {
                await _unitOfWork.Comments.DeleteAsync(comment.Id);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new BadRequestException("You can't delete other users comments");
            }
        }
    }
}
