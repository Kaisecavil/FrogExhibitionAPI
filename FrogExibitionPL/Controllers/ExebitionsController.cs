using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FrogExhibitionBLL.DTO.ExhibitionDTOs;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExhibitionsController : ControllerBase
    {
        private readonly ILogger<ExhibitionsController> _logger;
        private readonly IExhibitionService _exebitionService;

        public ExhibitionsController(ILogger<ExhibitionsController> logger, IExhibitionService exebitionService)
        {
            _logger = logger;
            _exebitionService = exebitionService;
        }

        // GET: api/Exhibitions
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ExhibitionDtoDetail>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ExhibitionDtoDetail>>> GetExhibitions()
        {
            try
            {
                return base.Ok(await _exebitionService.GetAllExhibitions());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        [HttpGet("sort/{sortParams}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ExhibitionDtoDetail>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ExhibitionDtoDetail>>> GetSortedExhibitions(string sortParams = " ")
        {
            try
            {
                return base.Ok(await _exebitionService.GetAllExhibitions(sortParams));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        // GET: api/Exhibitions/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ExhibitionDtoDetail))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ExhibitionDtoDetail>> GetExhibition(Guid id)
        {
            try
            {
                return base.Ok(await _exebitionService.GetExhibition(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // PUT: api/Exhibitions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutExhibition(Guid id, ExhibitionDtoForCreate exebition)
        {
            try
            {
                //ModelState.IsValid
                await _exebitionService.UpdateExhibition(id, exebition);
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

        // POST: api/Exhibitions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(IEnumerable<ExhibitionDtoDetail>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ExhibitionDtoDetail>> PostExhibition(ExhibitionDtoForCreate exebition)
        {
            try
            {
                var createdExhibition = await _exebitionService.CreateExhibition(exebition);
                return base.CreatedAtAction("GetExhibition", new { id = createdExhibition.Id }, createdExhibition);
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

        // DELETE: api/Exhibitions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExhibition(Guid id)
        {
            try
            {
                await _exebitionService.DeleteExhibition(id);
                return base.NoContent();
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
        }

        // GET: api/Exhibitions/5
        [HttpGet("rating/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<FrogDtoRating>>> GetRating(Guid id)
        {
            try
            {
                return base.Ok(await _exebitionService.GetRating(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }
    }
}
