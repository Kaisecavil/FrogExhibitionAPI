using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.DTO.FrogStarRatingDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.FrogStarRatingViewModels;
using FrogExhibitionPL.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrogStarRatingsController : ControllerBase
    {
        private readonly IFrogStarRatingService _frogStarRatingService;

        public FrogStarRatingsController(IFrogStarRatingService frogStarRatingService)
        {
            _frogStarRatingService = frogStarRatingService;
        }

        // GET: api/FrogStarRatings
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FrogStarRatingGeneralViewModel>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<ActionResult<IEnumerable<FrogStarRatingGeneralViewModel>>> GetFrogStarRatings()
        {
            try
            {
                return Ok(await _frogStarRatingService.GetAllFrogStarRatingsAsync());
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/FrogStarRatings/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FrogStarRatingGeneralViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<ActionResult<FrogStarRatingGeneralViewModel>> GetFrogStarRating(Guid id)
        {
            try
            {
                return Ok(await _frogStarRatingService.GetFrogStarRatingAsync(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/FrogStarRatings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<IActionResult> PutFrogStarRating(FrogStarRatingDtoForUpdate frogStarRating)
        {
            try
            {
                await _frogStarRatingService.UpdateFrogStarRatingAsync(frogStarRating);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ForbidException ex)
            {
                return Forbid(ex.Message);
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

        // POST: api/FrogStarRatings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(FrogStarRatingGeneralViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<ActionResult<FrogStarRatingGeneralViewModel>> PostFrogStarRating(FrogStarRatingDtoForCreate frogStarRating)
        {
            try
            {
                var createdFrogStarRatingId = await _frogStarRatingService.CreateFrogStarRatingAsync(frogStarRating);
                return CreatedAtAction("GetFrogStarRating", new { id = createdFrogStarRatingId }, createdFrogStarRatingId);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        // DELETE: api/FrogStarRatings/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<IActionResult> DeleteFrogStarRating(Guid id)
        {
            try
            {
                await _frogStarRatingService.DeleteFrogStarRatingAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ForbidException ex)
            {
                return Forbid(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
