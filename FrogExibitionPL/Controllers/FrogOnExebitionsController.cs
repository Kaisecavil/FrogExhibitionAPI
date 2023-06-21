using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.FrogOnExhibitionViewModels;
using FrogExhibitionBLL.Exceptions;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrogOnExhibitionsController : ControllerBase
    {
        private readonly ILogger<FrogOnExhibitionsController> _logger;
        private readonly IFrogOnExhibitionService _frogOnExhibitionService;

        public FrogOnExhibitionsController(ILogger<FrogOnExhibitionsController> logger, IFrogOnExhibitionService frogOnExhibitionService)
        {
            _logger = logger;
            _frogOnExhibitionService = frogOnExhibitionService;
        }

        // GET: api/FrogOnExhibitions
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FrogOnExhibitionDetailViewModel>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<FrogOnExhibitionDetailViewModel>>> GetFrogOnExhibitions()
        {
            try
            {
                return base.Ok(await _frogOnExhibitionService.GetAllFrogOnExhibitions());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        // GET: api/FrogOnExhibitions/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FrogOnExhibitionDetailViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<FrogOnExhibitionDetailViewModel>> GetFrogOnExhibition(Guid id)
        {
            try
            {
                return base.Ok(await _frogOnExhibitionService.GetFrogOnExhibition(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // PUT: api/FrogOnExhibitions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutFrogOnExhibition(Guid id, FrogOnExhibitionDtoForCreate frogOnExhibition)
        {
            try
            {
                //ModelState.IsValid
                //ModelState.AddModelError("")
                await _frogOnExhibitionService.UpdateFrogOnExhibition(id, frogOnExhibition);
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FrogOnExhibitionDetailViewModel>> PostFrogOnExhibition(FrogOnExhibitionDtoForCreate frogOnExhibition)
        {
            try
            {
                var createdFrogOnExhibition = await _frogOnExhibitionService.CreateFrogOnExhibition(frogOnExhibition);
                return base.CreatedAtAction("GetFrogOnExhibition", new { id = createdFrogOnExhibition.Id }, createdFrogOnExhibition);
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

        // DELETE: api/FrogOnExhibitions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFrogOnExhibition(Guid id)
        {
            try
            {
                await _frogOnExhibitionService.DeleteFrogOnExhibition(id);
                return base.NoContent();
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
        }
    }

}
