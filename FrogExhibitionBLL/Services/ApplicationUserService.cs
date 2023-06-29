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

namespace FrogExhibitionBLL.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUserService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;

        public ApplicationUserService(IUnitOfWork unitOfWork,
            ILogger<ApplicationUserService> logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _userProvider = userProvider;
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
    }
}

