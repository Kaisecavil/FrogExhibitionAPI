using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.FrogOnExhibitionViewModels;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Constants;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrogOnExhibitionsController : ControllerBase
    {
        private readonly IFrogOnExhibitionService _frogOnExhibitionService;

        public FrogOnExhibitionsController(IFrogOnExhibitionService frogOnExhibitionService)
        {
            _frogOnExhibitionService = frogOnExhibitionService;
        }

        // GET: api/FrogOnExhibitions
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FrogOnExhibitionDetailViewModel>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<ActionResult<IEnumerable<FrogOnExhibitionDetailViewModel>>> GetFrogOnExhibitions()
        {
            try
            {
                return base.Ok(await _frogOnExhibitionService.GetAllFrogOnExhibitionsAsync());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        // GET: api/FrogOnExhibitions/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FrogOnExhibitionDetailViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]

        public async Task<ActionResult<FrogOnExhibitionDetailViewModel>> GetFrogOnExhibition(Guid id)
        {
            try
            {
                return base.Ok(await _frogOnExhibitionService.GetFrogOnExhibitionAsync(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // PUT: api/FrogOnExhibitions/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> PutFrogOnExhibition(FrogOnExhibitionDtoForUpdate frogOnExhibition)
        {
            try
            {
                await _frogOnExhibitionService.UpdateFrogOnExhibitionAsync(frogOnExhibition);
                return base.NoContent();
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

        // POST: api/FrogOnExhibitions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(FrogOnExhibitionDetailViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> PostFrogOnExhibition(FrogOnExhibitionDtoForCreate frogOnExhibition)
        {
            try
            {
                var createdFrogOnExhibitionId = await _frogOnExhibitionService.CreateFrogOnExhibitionAsync(frogOnExhibition);
                return base.CreatedAtAction("GetFrogOnExhibition", new { createdFrogOnExhibitionId }, createdFrogOnExhibitionId);
            }
            catch (BadRequestException ex)
            {
                return base.BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return base.UnprocessableEntity(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return base.UnprocessableEntity(ex.Message);
            }

        }

        // DELETE: api/FrogOnExhibitions/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> DeleteFrogOnExhibition(Guid id)
        {
            try
            {
                await _frogOnExhibitionService.DeleteFrogOnExhibitionAsync(id);
                return base.NoContent();
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
        }
    }

}
