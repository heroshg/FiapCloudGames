using FiapCloudGames.Application.Commands.PurchaseGameLicense;
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
    public class GameLicensesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly BaseLogger<GameLicensesController> _logger;

        public GameLicensesController(
            IMediator mediator,
            BaseLogger<GameLicensesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Purchase a game license
        /// </summary>
        /// <remarks>
        /// Allows an authenticated user to purchase a game license.
        ///
        /// Business rules:
        /// - The user must be authenticated
        /// - The game must exist
        /// - The user must have sufficient balance
        /// </remarks>
        /// <param name="model">Data required to purchase a game license</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <response code="200">Game license purchased successfully</response>
        /// <response code="400">Business rule or validation error</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PurchaseGameLicense(
            [FromBody] PurchaseGameLicenseCommand model,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogError(
                    $"PurchaseGameLicense failed. Reason={result.Message}"
                );

                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
