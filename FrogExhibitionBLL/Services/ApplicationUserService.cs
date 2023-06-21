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

namespace FrogExhibitionBLL.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUserService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public ApplicationUserService(IUnitOfWork unitOfWork, ILogger<ApplicationUserService> logger, IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task<IEnumerable<ApplicationUserDtoDetail>> GetAllApplicationUsers()
        {
            if (_userManager.Users.IsNullOrEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var result = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<ApplicationUserDtoDetail>>(result);
        }

        public async Task<ApplicationUserDtoDetail> GetApplicationUser(Guid id)
        {
            if (_userManager.Users.IsNullOrEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var applicationUser = await _userManager.Users.Where(u => u.Id == id.ToString()).FirstOrDefaultAsync();

            if (applicationUser == null)
            {
                throw new NotFoundException("Entity not found");
            }

            return _mapper.Map<ApplicationUserDtoDetail>(applicationUser);
        }

        public async Task UpdateApplicationUser(Guid id, ApplicationUserDtoForUpdate applicationUser)
        {
            try
            {
                var user = await _userManager.Users.Where(u => u.Id == id.ToString()).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new NotFoundException("Entity not found");
                }
                user.PhoneNumber = applicationUser.PhoneNumber;
                user.UserName = applicationUser.UserName;
                user.Email = applicationUser.Email;
                var result = await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userManager.Users.AnyAsync(u => u.Id == id.ToString()))
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

        public async Task DeleteApplicationUser(Guid id)
        {

            if (_userManager.Users.IsNullOrEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var applicationUser = await _userManager.Users.Where(u => u.Id == id.ToString()).FirstOrDefaultAsync();

            if (applicationUser == null)
            {
                throw new NotFoundException("Entity not found");
            }

            await _userManager.DeleteAsync(applicationUser);
        }
    }
}

