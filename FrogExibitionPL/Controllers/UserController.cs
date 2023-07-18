using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;
using FrogExhibitionPL.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IApplicationUserService _userService;

        public UsersController(IApplicationUserService userService)
        {
            _userService = userService;

        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ApplicationUserGeneralViewModel>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [AuthorizeRoles(RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<ActionResult<IEnumerable<ApplicationUserGeneralViewModel>>> GetUsers()
        {
            try
            {
                return Ok(await _userService.GetAllApplicationUsersAsync());
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/Users/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationUserDetailViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<ActionResult<ApplicationUserDetailViewModel>> GetUser(Guid id)
        {
            try
            {
                return Ok(await _userService.GetApplicationUserAsync(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Users/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<IActionResult> PutUser(ApplicationUserDtoForUpdate user)
        {
            try
            {
                await _userService.UpdateApplicationUserAsync(user);
                return NoContent();
            }
            catch (ForbidException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        // DELETE: api/Users/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteApplicationUserAsync(id);
                return NoContent();
            }
            catch (ForbidException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/Users/stat/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("stat/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [AuthorizeRoles(RoleConstants.AdminRole, RoleConstants.UserAdminRole)]
        public async Task<IActionResult> GetUserStatictics(Guid id, bool createExcelReport = true)
        {
            try
            {
                return createExcelReport ?
                    await _userService.GetUserStatisticsReportAsync(id) :
                    Ok(await _userService.GetUserStatisticsAsync(id));
            }
            catch (ForbidException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/Users/isUserPsychic/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("isUserPsychic/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [AuthorizeRoles(RoleConstants.AdminRole, RoleConstants.UserAdminRole)]
        public async Task<IActionResult> GetUserLastVotesOnExhibitions(Guid id, int quantityOfLastExhibitions)
        {
            try
            {
                return Ok(await _userService.GetUserLastVotesOnExhibitionsAsync(id, quantityOfLastExhibitions));
            }
            catch (ForbidException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}

