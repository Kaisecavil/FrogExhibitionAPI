﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.ExhibitionViewModels;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Constants;
using System;
using FrogExhibitionBLL.Interfaces.IHelper;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExhibitionsController : ControllerBase
    {
        private readonly IExhibitionService _exebitionService;
        private readonly IExcelHelper _excelHelper;

        public ExhibitionsController(IExhibitionService exebitionService,
            IExcelHelper excelHelper)
        {
            _exebitionService = exebitionService;
            _excelHelper = excelHelper;
        }

        // GET: api/Exhibitions
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ExhibitionGeneralViewModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ExhibitionGeneralViewModel>>> GetExhibitions()
        {
            try
            {
                return base.Ok(await _exebitionService.GetAllExhibitionsAsync());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        [HttpGet("sort/{sortParams}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ExhibitionGeneralViewModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ExhibitionGeneralViewModel>>> GetSortedExhibitions(string sortParams = "name desc,country")
        {
            try
            {
                return base.Ok(await _exebitionService.GetAllExhibitionsAsync(sortParams));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }

        }

        // GET: api/Exhibitions/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ExhibitionDetailViewModel))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ExhibitionDetailViewModel>> GetExhibition(Guid id)
        {
            try
            {
                return base.Ok(await _exebitionService.GetExhibitionAsync(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // PUT: api/Exhibitions/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> PutExhibition(ExhibitionDtoForUpdate exebition)
        {
            try
            {
                await _exebitionService.UpdateExhibitionAsync(exebition);
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> PostExhibition(ExhibitionDtoForCreate exebition)
        {
            try
            {
                var createdExhibitionId = await _exebitionService.CreateExhibitionAsync(exebition);
                return base.CreatedAtAction("GetExhibition", new { id = createdExhibitionId }, createdExhibitionId);
            }
            catch (BadRequestException ex)
            {
                return base.BadRequest(ex.Message);
            }
        }

        // DELETE: api/Exhibitions/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles =  RoleConstants.AdminRole)]
        public async Task<IActionResult> DeleteExhibition(Guid id)
        {
            try
            {
                await _exebitionService.DeleteExhibitionAsync(id);
                return base.NoContent();
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }
        }

        // GET: api/Exhibitions/rating/176223D5-5073-4961-B4EF-ECBE41F1A0C6
        [HttpGet("rating/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Roles = RoleConstants.UserRole)]
        public async Task<ActionResult<IEnumerable<FrogRatingViewModel>>> GetRating(Guid id)
        {
            try
            {
                return base.Ok(await _exebitionService.GetRatingAsync(id));
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        // GET: api/Exhibitions/history
        [HttpGet("history")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Authorize(Roles = RoleConstants.UserRole)]
        public async Task<ActionResult<IEnumerable<FrogRatingViewModel>>> GetHistory()
        {
            try
            {
                return base.Ok(await _exebitionService.GetBestFrogsHistoryAsync());
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        // GET: api/Exhibitions/report
        [HttpGet("report/{id}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<IActionResult> GetReport(Guid id)
        {
            try
            {
                return await _exebitionService.GetExhibitionExcelReportAsync(id);
            }
            catch (NotFoundException ex)
            {
                return base.NotFound(ex.Message);
            }


        }
    }
}
