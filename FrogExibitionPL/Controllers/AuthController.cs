using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrogExhibitionDAL.Models;
using FrogExhibitionBLL.Interfaces.IService;

namespace FrogExhibitionPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model");
            }
            if(await _authService.LoginAsync(user))
            {
                var appUser = _userManager.Users.FirstOrDefault(u => u.Email == user.Email);
                var roles = await _userManager.GetRolesAsync(appUser);
                var tokenString = _authService.GenerateTokenString(user,roles);
                return Ok(tokenString);
            }
            return BadRequest("Wrong username or password");
        }

        [HttpPost("Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register(LoginUser user)
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
