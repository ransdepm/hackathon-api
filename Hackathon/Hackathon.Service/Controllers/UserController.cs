using Hackathon.Service.Models;
using Hackathon.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Hackathon.Service.Configuration;
using Hackathon.Entities;
using System.Security.Claims;
using Hackathon.Service.Entities;

namespace Hackathon.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        protected readonly ILogger<UserController> Logger;

        public UserController(IUserService userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            Logger = logger;
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(User), 200)]
        [HttpPost("admin")]
        public IActionResult CreateUser([FromBody]AdminUserModel model)
        {
            if (_userService.ActiveUserExists(model.Email))
                return BadRequest("Active user already exist in the system with that email address");

            var user = _userService.CreateAdminUser(model.Email, model.Password);

            return Ok(user);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(User), 200)]
        [HttpPost("admin/login")]
        public IActionResult AdminLogin([FromBody]AdminUserLoginModel model)
        {
            var user = _userService.LoginAdminUser(model);

            if (user == null)
            {
                Logger?.LogInformation("time: {0} User:{1} Message: Invalid login attempt", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), model.Email);
                return Unauthorized("Email or password is incorrect");
            }

            return Ok(user);
        }

        [Authorize]
        [ProducesResponseType(typeof(User), 200)]
        [HttpGet("me")]
        public IActionResult GetUser()
        {
            var currentUser = _userService.GetCurrentAdminUser(HttpContext.User.Identity as ClaimsIdentity);

            if (currentUser == null)
            {
                return Unauthorized("No user information for this user.");
            }

            return Ok(currentUser);
        }
    }
}
