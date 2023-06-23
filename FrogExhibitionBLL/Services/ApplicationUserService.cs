using AutoMapper;
using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using FrogExhibitionBLL.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;

namespace FrogExhibitionBLL.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUserService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public ApplicationUserService(IUnitOfWork unitOfWork,
            ILogger<ApplicationUserService> logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _httpContext = httpContext;
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
            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (applicationUser == null)
            {
                throw new NotFoundException("Entity not found");
            }
            await _userManager.DeleteAsync(applicationUser);
            await _unitOfWork.SaveAsync();
        }
    }
}

