using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrogExhibitionDAL.Models;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login(ApplicationUserDtoForLogin user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model");
            }
            try
            {
                return Ok(await _authService.LoginAsync(user));
            }
            catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register(ApplicationUserDtoForLogin user)
        {
            try
            {
                if (await _authService.RegisterUserAsync(user))
                {
                    return Ok("Successful registration");
                }
                return BadRequest("smth went wrong");
            }
            catch(DbUpdateException ex)
            {
               return BadRequest(ex.Message);
            }
        }
    }
}
