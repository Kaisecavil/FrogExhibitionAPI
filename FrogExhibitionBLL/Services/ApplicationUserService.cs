using AutoMapper;
using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using FrogExhibitionBLL.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;
using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.Interfaces.IProvider;
using FrogExhibitionDAL.Enums;
using Microsoft.AspNetCore.Mvc;
using FrogExhibitionBLL.Interfaces.IHelper;
using FrogExhibitionBLL.ViewModels.VoteViewModels;
using FrogExhibitionBLL.ViewModels.CommentViewModels;

namespace FrogExhibitionBLL.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUserService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;
        private readonly IFileHelper _fileHelper;
        private readonly IExcelHelper _excelHelper;
        private readonly IExhibitionService _exhibitionService;

        public ApplicationUserService(IUnitOfWork unitOfWork,
            ILogger<ApplicationUserService> logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUserProvider userProvider,
            IFileHelper fileHelper,
            IExcelHelper excelHelper,
            IExhibitionService exhibitionService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _userProvider = userProvider;
            _fileHelper = fileHelper;
            _excelHelper = excelHelper;
            _exhibitionService = exhibitionService;
        }

        public async Task<IEnumerable<ApplicationUserDetailViewModel>> GetAllApplicationUsersAsync()
        {
            var result = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<ApplicationUserDetailViewModel>>(result);
        }

        public async Task<ApplicationUserDetailViewModel> GetApplicationUserAsync(Guid id)
        {
            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (applicationUser == null)
            {
                throw new NotFoundException("Entity not found");
            }
            return _mapper.Map<ApplicationUserDetailViewModel>(applicationUser);
        }

        public async Task UpdateApplicationUserAsync(ApplicationUserDtoForUpdate applicationUser)
        {
            var currentUserEmail = _userProvider.GetUserEmail();
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            if (currentUser.Id == applicationUser.Id.ToString() ||
                await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdminRole))
            {
                try
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
                    if (user == null)
                    {
                        throw new NotFoundException("Entity not found");
                    }
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.UserName = applicationUser.UserName;
                    user.Email = applicationUser.Email;
                    UserKnowledgeLevel knowledgeLevel;
                    if(Enum.TryParse(applicationUser.KnowledgeLevel,out knowledgeLevel))
                    {
                        user.KnowledgeLevel = knowledgeLevel;
                        var result = await _userManager.UpdateAsync(user);
                        await _unitOfWork.SaveAsync();
                    }
                    else
                    {
                        throw new BadRequestException("Knowledge level is invalid");
                    }

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _userManager.Users.AnyAsync(u => u.Id == applicationUser.Id))
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
                    _logger.LogError(ex.Message, applicationUser);
                    throw;
                };
            }
            else
            {
                throw new ForbidException("Access denied");
            }
            
        }

        public async Task DeleteApplicationUserAsync(Guid id)
        {
            var currentUserEmail = _userProvider.GetUserEmail();
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            if (currentUser.Id == id.ToString() ||
                await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdminRole))
            {
                var applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
                if (applicationUser == null)
                {
                    throw new NotFoundException("Entity not found");
                }
                await _userManager.DeleteAsync(applicationUser);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new ForbidException("Access denied");
            }
            
        }

        public async Task<FileContentResult> GetUserStatisticsAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (user == null)
            {
                throw new NotFoundException("Entity not found");
            }

            string filePath = _fileHelper.GetUserReportFilePath(user.Id);
            try
            {
                var votes = (await GetUserVotesExcelReportDataAsync(user)).Select(o => o.Result).ToList<object>();
                var comments = GetUserCommentsExcelReportData(user).ToList<object>();
                _excelHelper.CreateSpreadsheetFromObjects(votes, filePath, "User Votes");
                _excelHelper.AppendObjectsToSpreadsheet(comments, filePath,"User Comments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            if (!File.Exists(filePath))
            {
                throw new NotFoundException("a");
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            string contentType = "application/octet-stream";
            string fileName = Path.GetFileName(filePath);
            var fileContentResult = new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = fileName
            };

            return fileContentResult;
        }

        private async Task<IEnumerable<Task<VoteExcelReportViewModel>>> GetUserVotesExcelReportDataAsync(ApplicationUser user)
        {
            var votes = user.Votes;
            var res = votes.Select(async v => new VoteExcelReportViewModel()
            {
                ApplicationUserName = user.UserName,
                ExhibitionName = v.FrogOnExhibition.Exhibition.Name,
                FrogColor = v.FrogOnExhibition.Frog.Color,
                FrogGenus = v.FrogOnExhibition.Frog.Genus,
                FrogSpecies = v.FrogOnExhibition.Frog.Species,
                FrogsPlaceOnExhibition = await _exhibitionService.GetFrogsPlaceOnExhibitionAsync(v.FrogOnExhibition.FrogId, v.FrogOnExhibition.ExhibitionId)
            });
            return res;
        }

        private IEnumerable<CommentExcelReportViewModel> GetUserCommentsExcelReportData(ApplicationUser user)
        {
            var comments = user.Comments;
            var res = comments.Select(c => new CommentExcelReportViewModel()
            {
                ApplicationUserName = user.UserName,
                CreationDate = c.CreationDate.ToString(),
                ExhibitionName = c.FrogOnExhibition.Exhibition.Name,
                FrogGenus = c.FrogOnExhibition.Frog.Genus,
                FrogSpecies = c.FrogOnExhibition.Frog.Species,
                FrogColor = c.FrogOnExhibition.Frog.Color,
                Text = c.Text
            });
            return res;
        }
    }
}

