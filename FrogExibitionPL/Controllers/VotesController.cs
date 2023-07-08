using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.DTO.VoteDtos;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.VoteViewModels;
using FrogExhibitionPL.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VotesController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        // GET: api/Votes
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VoteDetailViewModel>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<ActionResult<IEnumerable<VoteDetailViewModel>>> GetVotes()
        {
            try
            {
                return base.Ok(await _voteService.GetAllVotesAsync());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        // GET: api/Votes/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(VoteDetailViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<ActionResult<VoteDetailViewModel>> GetVote(Guid id)
        {
            try
            {
                return base.Ok(await _voteService.GetVoteAsync(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // PUT: api/Votes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> PutVote(VoteDtoForUpdate vote)
        {
            try
            {
                await _voteService.UpdateVoteAsync(vote);
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
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<ActionResult<VoteDetailViewModel>> PostVote(VoteDtoForCreate vote)
        {
            try
            {
                var createdVoteId = await _voteService.CreateVoteAsync(vote);
                return base.CreatedAtAction("GetVote", new { id = createdVoteId }, createdVoteId);
            }
            catch (BadRequestException ex)
            {
                return base.BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return base.UnprocessableEntity(ex.Message);
            }
        }

        // DELETE: api/Votes/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.AdminRole, RoleConstants.UserAdminRole)]
        public async Task<IActionResult> DeleteVote(Guid id)
        {
            try
            {
                await _voteService.DeleteVoteAsync(id);
                return base.NoContent();
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
            catch (ForbidException ex)
            {
                return base.Forbid(ex.Message);
            }
        }
    }
}
