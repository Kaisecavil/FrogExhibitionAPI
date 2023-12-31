﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionBLL.DTO.FrogDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Constants;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrogsController : ControllerBase
    {
        private readonly IFrogService _frogService;

        public FrogsController(IFrogService frogService)
        {
            _frogService = frogService;
        }

        // GET: api/Frogs
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FrogGeneralViewModel>))]
        public async Task<ActionResult<IEnumerable<FrogGeneralViewModel>>> GetFrogs()
        {
            return Ok(await _frogService.GetAllFrogsAsync());
        }

        // GET: api/Frogs/?sortParams=genus
        [HttpGet("sort/{sortParams}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FrogGeneralViewModel>))]
        public async Task<ActionResult<IEnumerable<FrogGeneralViewModel>>> GetSortedFrogs(string sortParams = "sex,genus desc")
        {
            return Ok(await _frogService.GetAllFrogsAsync(sortParams));
        }

        // GET: api/Frogs/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FrogDetailViewModel))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<FrogDetailViewModel>> GetFrog(Guid id)
        {
            try
            {
                return Ok(await _frogService.GetFrogAsync(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Frogs/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> PutFrog([FromForm]FrogDtoForUpdate frog) 
        {
            try
            {
                await _frogService.UpdateFrogAsync(frog);
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

        // POST: api/Frogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(FrogDetailViewModel))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> PostFrog([FromForm]FrogDtoForCreate frog)
        {
            try
            {
                var createdFrogId = await _frogService.CreateFrogAsync(frog);
                return CreatedAtAction("GetFrog", new { id = createdFrogId }, createdFrogId);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Frogs/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> DeleteFrog(Guid id)
        {
            try
            {
                await _frogService.DeleteFrogAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
