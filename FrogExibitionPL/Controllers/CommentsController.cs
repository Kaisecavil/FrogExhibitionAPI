using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.DTO.CommentsDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.CommentViewModels;
using FrogExhibitionPL.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: api/Comments
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CommentGeneralViewModel>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<ActionResult<IEnumerable<CommentGeneralViewModel>>> GetComments()
        {
            try
            {
                return Ok(await _commentService.GetAllCommentsAsync());
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        // GET: api/Comments/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CommentGeneralViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<ActionResult<CommentGeneralViewModel>> GetComment(Guid id)
        {
            try
            {
                return Ok(await _commentService.GetCommentAsync(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }


        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<IActionResult> PutComment(CommentDtoForUpdate comment)
        {
            try
            {
                await _commentService.UpdateCommentAsync(comment);
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CommentGeneralViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<ActionResult<CommentGeneralViewModel>> PostComment(CommentDtoForCreate comment)
        {
            try
            {
                var createdCommentId = await _commentService.CreateCommentAsync(comment);
                return CreatedAtAction("GetComment", new { id = createdCommentId }, createdCommentId);
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

        // DELETE: api/Comments/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [AuthorizeRoles(RoleConstants.UserRole, RoleConstants.UserAdminRole, RoleConstants.AdminRole)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
