using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;
using FrogExhibitionDAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IApplicationUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserProvider _userProvider;

        public UsersController(ILogger<UsersController> logger, IApplicationUserService userService, UserManager<ApplicationUser> userManager, IUserProvider userProvider)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
            _userProvider = userProvider;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ApplicationUserGeneralViewModel>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ApplicationUserGeneralViewModel>>> GetUsers()
        {
            try
            {
                return base.Ok(await _userService.GetAllApplicationUsers());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationUserDetailViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApplicationUserDetailViewModel>> GetUser(Guid id)
        {
            try
            {
                return base.Ok(await _userService.GetApplicationUser(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PutUser(Guid id, ApplicationUserDtoForUpdate user)
        {
            try
            {
                var currentUserEmail = _userProvider.GetUserEmail();
                var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
                if (currentUser.Id == id.ToString() || await _userManager.IsInRoleAsync(currentUser,"Admin"))
                {
                    //ModelState.IsValid
                    //ModelState.AddModelError("")
                    await _userService.UpdateApplicationUser(id, user);
                    return base.NoContent();
                }
                else
                {
                    return base.Unauthorized("Access denied");
                }
                
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return base.BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return base.UnprocessableEntity(ex.Message);
            }

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var currentUserEmail = _userProvider.GetUserEmail();
                var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
                if (currentUser.Id == id.ToString() || await _userManager.IsInRoleAsync(currentUser, "Admin"))
                {
                    await _userService.DeleteApplicationUser(id);
                    return base.NoContent();
                }
                else
                {
                    return base.Unauthorized("Access denied");
                }

            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
        }
    }
}
