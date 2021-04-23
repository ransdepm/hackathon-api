using Hackathon.Entities;
using Hackathon.Service.Models;
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
        public IActionResult GetBaseballGames()
        {
            var message = _gameService.GetGames();
            return Ok(message);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(MoundGame), 200)]
        [HttpGet("{id}/moundGame")]
        public IActionResult GetMoundGame(int id)
        {
            var message = _gameService.GetMoundGame(id);
            return Ok(message);
        }

        [AllowAnonymous]
        [HttpPut("moundResult/{id}/lock")]
        public IActionResult LockMoundResult(int id)
        {
            _gameService.LockMoundResult(id);
            return Ok();
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(MoundGameResult), 200)]
        [HttpPut("moundResult/{id}/result")]
        public IActionResult ScoreMoundResult(int id, MoundResultModel model)
        {
            var moundResult = _gameService.GetMoundResultById(id);

            if (moundResult == null)
                return BadRequest("Mound Result Id does not exist.");
            if (moundResult.Status == "COMPLETED")
                return BadRequest("Result already submitted for this round.");

            var result = _gameService.StoreMoundResult(id, model);
            return Ok(result);
        }


        [AllowAnonymous]
        [ProducesResponseType(typeof(MoundGameResult), 200)]
        [HttpPost("moundResult/{id}/submit")]
        public IActionResult UserSubmitMoundResult(int id, UserResultModel model)
        {
            var moundResult = _gameService.GetMoundResultById(id);
            var gameUser = _userService.GetGameUserByName(model.Username);

            if (gameUser == null)
                return BadRequest("No user with that user name.");

            if (moundResult == null)
                return BadRequest("Mound Result Id does not exist.");

            if (moundResult.Status == "LOCKED")
                return BadRequest("Voting is currently locked.  Please wait until the next round starts.");

            if (moundResult.Status == "COMPLETED")
                return BadRequest("Voting is currently over for this round.  Please submit result to the next round.");


            var result = _gameService.StoreUserMoundResult(id, gameUser.Id, model);
            return Ok(result);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(List<UserMoundGameResult>), 200)]
        [HttpGet("moundGame/{moundGameId}/score")]
        public IActionResult GetUserMoundGameResults(int moundGameId, [FromQuery] GameResultsModel model)
        {
            var gameUser = _userService.GetGameUserByName(model.Username);

            if (gameUser == null)
                return BadRequest("No user with that user name.");


            var results = _gameService.GetMoundGameResultsForUser(moundGameId, gameUser.Id);
            return Ok(results);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(List<MoundGameTotals>), 200)]
        [HttpGet("moundGame/{moundGameId}/scores")]
        public IActionResult GetAllMoundGameResults(int moundGameId)
        {
            List<MoundGameTotals> results = _gameService.GetAllMoundGameResults(moundGameId);
            return Ok(results);
        }


    }
}
