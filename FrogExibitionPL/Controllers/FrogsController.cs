using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionBLL.DTO.FrogDTOs;
using FrogExhibitionBLL.Exceptions;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrogsController : ControllerBase
    {
        private readonly ILogger<FrogsController> _logger;
        private readonly IFrogService _frogService;

        public FrogsController(ILogger<FrogsController> logger, IFrogService frogService)
        {
            _logger = logger;
            _frogService = frogService;
        }

        // GET: api/Frogs
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FrogGeneralViewModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<FrogGeneralViewModel>>> GetFrogs()
        {
            try
            {
                return base.Ok(await _frogService.GetAllFrogs());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        [HttpGet("sort/{sortParams}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FrogGeneralViewModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<FrogGeneralViewModel>>> GetSortedFrogs(string sortParams = "sex,genus desc")
        {
            try
            {
                return base.Ok(await _frogService.GetAllFrogs(sortParams));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        // GET: api/Frogs/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FrogDetailViewModel))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<FrogDetailViewModel>> GetFrog(Guid id)
        {
            try
            {
                return base.Ok(await _frogService.GetFrog(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // PUT: api/Frogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutFrog(/*Guid id,*/ [FromForm]FrogDtoForUpdate frog) // накостылял? но работает , но вопрос норм ли это для апи?
        {
            try
            {
                //ModelState.IsValid
                //ModelState.AddModelError("")
                await _frogService.UpdateFrog(/*id,*/ frog);
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
        }

        // POST: api/Frogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(FrogDetailViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FrogDetailViewModel>> PostFrog([FromForm]FrogDtoForCreate frog)
        {
            try
            {
                var createdFrog = await _frogService.CreateFrog(frog);
                return base.CreatedAtAction("GetFrog", new { id = createdFrog.Id }, createdFrog);
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return base.BadRequest(ex.Message);
            }
        }

        // DELETE: api/Frogs/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFrog(Guid id)
        {
            try
            {
                await _frogService.DeleteFrog(id);
                return base.NoContent();
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
        }

    }
}
