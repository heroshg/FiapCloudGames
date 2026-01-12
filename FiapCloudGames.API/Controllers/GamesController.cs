using FiapCloudGames.Application.Commands.RegisterGame;
using FiapCloudGames.Application.Queries.GetAllGames;
using FiapCloudGames.Application.Queries.GetAllGamesByUserId;
using FiapCloudGames.Infrastructure.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly BaseLogger<GameLicensesController> _logger;

        public GamesController(
            IMediator mediator,
            BaseLogger<GameLicensesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new game
        /// </summary>
        /// <remarks>
        /// Registers a new game in the platform catalog.
        ///
        /// Business rules:
        /// - Only users with the <b>Admin</b> role are allowed
        /// - The game name must be unique
        /// - Game price must be greater than zero
        /// </remarks>
        /// <param name="model">Game registration data</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <response code="200">Game registered successfully</response>
        /// <response code="400">Validation or business rule error</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden – user is not an admin</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterGameCommand model,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError($"Error registering game: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves a paginated list of games
        /// </summary>
        /// <remarks>
        /// Returns a paginated list of games with optional filtering by name.
        ///
        /// Pagination:
        /// - Page index starts at 0
        /// - Page size default is 10
        /// </remarks>
        /// <param name="name">Optional game name filter</param>
        /// <param name="page">Page index (starting from 0)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <response code="200">Games retrieved successfully</response>
        /// <response code="400">Invalid query parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(
            [FromQuery] string name = "",
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default!)
        {
            var result = await _mediator.Send(
                new GetAllGamesQuery(name, page, pageSize),
                cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError($"Error getting games: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
        /// <summary>
        /// Retrieves all games owned by a specific user
        /// </summary>
        /// <remarks>
        /// Returns the list of games associated with the provided user identifier.
        /// The user must exist and have at least one associated game license.
        /// </remarks>
        /// <param name="userId">Unique identifier of the user</param>
        /// <param name="cancellation">Request cancellation token</param>
        /// <response code="200">Games retrieved successfully</response>
        /// <response code="400">Invalid user ID or no games found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("{userId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllGamesByUserId(Guid userId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(
                new GetAllGamesByUserIdQuery(userId),
                cancellation);

            if (!result.IsSuccess)
            {
                _logger.LogError($"Error getting games by user ID: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
