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
            try
            {
                var currentUserEmail = _userProvider.GetUserEmail();
                var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
                if (currentUser.Id == applicationUser.Id.ToString() &&
                    !(await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdminRole) ||
                        await _userManager.IsInRoleAsync(currentUser, RoleConstants.UserAdminRole)))
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
                    if (user == null)
                    {
                        throw new NotFoundException("Entity not found");
                    }
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.UserName = applicationUser.UserName;
                    user.Email = applicationUser.Email;
                    var result = await _userManager.UpdateAsync(user);
                    await _unitOfWork.SaveAsync();
                }
                else if (await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdminRole) ||
                    await _userManager.IsInRoleAsync(currentUser, RoleConstants.UserAdminRole))
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
                        if (Enum.TryParse(applicationUser.KnowledgeLevel, out knowledgeLevel))
                        {
                            user.KnowledgeLevel = knowledgeLevel;
                        }
                        else
                        {
                            throw new BadRequestException("Knowledge level is invalid");
                        }
                    var result = await _userManager.UpdateAsync(user);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    throw new BadRequestException("Access denied");
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
        public async Task DeleteApplicationUserAsync(Guid id)
        {
            var currentUserEmail = _userProvider.GetUserEmail();
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            if (currentUser.Id == id.ToString() ||
                await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdminRole)||
                await _userManager.IsInRoleAsync(currentUser, RoleConstants.UserAdminRole))
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

        public async Task<FileContentResult> GetUserStatisticsReportAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (user == null)
            {
                throw new NotFoundException("Entity not found");
            }

            string filePath = _fileHelper.GetUserReportFilePath(user.Id);
            try
            {
                var votes = (await GetUserVotesReportDataAsync(user)).ToList();
                var comments = GetUserCommentsReportData(user).ToList();
                _excelHelper.CreateSpreadsheetFromObjects(votes, filePath, "User Votes");
                _excelHelper.AppendObjectsToSpreadsheet(comments, filePath, "User Comments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return _fileHelper.GetFileContentResult(filePath, "application/octet-stream");
        }

        public async Task<ApplicationUserReportViewModel> GetUserStatisticsAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (user == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var votes = await GetUserVotesReportDataAsync(user);
            var comments = GetUserCommentsReportData(user);
            return new ApplicationUserReportViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Comments = comments.ToList(),
                Votes = votes.ToList()
            };


        }

        public async Task<bool> GetUserLastVotesOnExhibitionsAsync(Guid id, int quantityOfLastExhibitions)
        {
            if(quantityOfLastExhibitions <= 0)
            {
                throw new BadRequestException("Quantity Of Last Exhibitions can't be zero or less");
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (user == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var data = (await GetUserVotesReportDataAsync(user)).ToList();
            var res = data
                .GroupBy(d => d.ExhibitionName, (k, v) => new { key = k, value = v.ToList() })
                .Take(quantityOfLastExhibitions)
                .All(g => g.value.All(v => v.FrogsPlaceOnExhibition < 4));
            return res;
        }

        private async Task<IEnumerable<VoteReportViewModel>> GetUserVotesReportDataAsync(ApplicationUser user)
        {
            var votes = user.Votes;
            var res = votes.Select(async v => new VoteReportViewModel()
            {
                ApplicationUserName = user.UserName,
                ExhibitionName = v.FrogOnExhibition.Exhibition.Name,
                ExhibitionDate = v.FrogOnExhibition.Exhibition.Date.ToString(),
                FrogColor = v.FrogOnExhibition.Frog.Color,
                FrogGenus = v.FrogOnExhibition.Frog.Genus,
                FrogSpecies = v.FrogOnExhibition.Frog.Species,
                FrogsPlaceOnExhibition = await _exhibitionService.GetFrogsPlaceOnExhibitionAsync(v.FrogOnExhibition.FrogId, v.FrogOnExhibition.ExhibitionId)
            });
            return res.Select(o => o.Result).ToList();
        }

        private IEnumerable<CommentReportViewModel> GetUserCommentsReportData(ApplicationUser user)
        {
            var comments = user.Comments;
            var res = comments.Select(c => new CommentReportViewModel()
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

