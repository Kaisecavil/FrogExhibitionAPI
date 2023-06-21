using FrogExhibitionBLL.DTO.VoteDtos;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.VoteViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly ILogger<VotesController> _logger;
        private readonly IVoteService _voteService;

        public VotesController(ILogger<VotesController> logger, IVoteService voteService)
        {
            _logger = logger;
            _voteService = voteService;
        }

        // GET: api/Votes
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VoteDetailViewModel>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<VoteDetailViewModel>>> GetVotes()
        {
            try
            {
                return base.Ok(await _voteService.GetAllVotes());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        // GET: api/Votes/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(VoteDetailViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<VoteDetailViewModel>> GetVote(Guid id)
        {
            try
            {
                return base.Ok(await _voteService.GetVote(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // PUT: api/Votes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutVote(Guid id, VoteDtoForCreate vote)
        {
            try
            {
                //ModelState.IsValid
                //ModelState.AddModelError("")
                await _voteService.UpdateVote(id, vote);
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

        // POST: api/Votes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(VoteDetailViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<VoteDetailViewModel>> PostVote(VoteDtoForCreate vote)
        {
            try
            {
                var createdVote = await _voteService.CreateVote(vote);
                return base.CreatedAtAction("GetVote", new { id = createdVote.Id }, createdVote);
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

        // DELETE: api/Votes/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVote(Guid id)
        {
            try
            {
                await _voteService.DeleteVote(id);
                return base.NoContent();
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
        }
    }
}
