using Hackathon.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hackathon.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        protected readonly ILogger<AdminController> Logger;

        public AdminController(IUserService userService,
            IAdminService adminService,
            ILogger<AdminController> logger)
        {
            _userService = userService;
            _adminService = adminService;
            Logger = logger;
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet("test")]
        public IActionResult GetTest()
        {
            var message = _adminService.GetMessage();
            return Ok(message);
        }

        
    }
}
