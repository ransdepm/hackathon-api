using Hackathon.Entities;
using Hackathon.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Hackathon.Service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        protected readonly ILogger<GameController> Logger;

        public GameController(IUserService userService,
            IGameService gameService,
            ILogger<GameController> logger)
        {
            _userService = userService;
            _gameService = gameService;
            Logger = logger;
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(List<BaseballGame>), 200)]
        [HttpGet("all")]
        public IActionResult GetTest()
        {
            var message = _gameService.GetGames();
            return Ok(message);
        }

        
    }
}
