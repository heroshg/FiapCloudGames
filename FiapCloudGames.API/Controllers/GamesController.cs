using FiapCloudGames.Application.Commands.RegisterGame;
using FiapCloudGames.Application.Queries.GetAllGames;
using FiapCloudGames.Infrastructure.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly BaseLogger<GameLicensesController> _logger;

        public GamesController(IMediator mediator, BaseLogger<GameLicensesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterGameCommand model, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);

            if(!result.IsSuccess)
            {
                _logger.LogError($"Error registering game: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string name = "", [FromQuery] int page = 0, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default!)
        {
            var result = await _mediator.Send(new GetAllGamesQuery(name, page, pageSize), cancellationToken);
            if(!result.IsSuccess)
            {
                _logger.LogError($"Error getting games: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

    }
}
