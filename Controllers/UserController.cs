using Microsoft.AspNetCore.Mvc;
using retail_management.Dtos;
using retail_management.Services;

namespace retail_management.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // User login
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginInputDto user)
        {
            var dbUser = await _userService.LoginAsync(user);

            if (dbUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"incorrect username or pasword");
            }

            return CreatedAtAction("Login", new { id = dbUser.id }, dbUser);
        }

        // User registration need to implement
    }
}
