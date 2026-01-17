using FiapCloudGames.Application.Commands.PurchasePromotion;
using FiapCloudGames.Application.Commands.RegisterPromotion;
using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using FiapCloudGames.Infrastructure.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PromotionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly BaseLogger<PromotionsController> _logger;

        public PromotionsController(
            IMediator mediator,
            BaseLogger<PromotionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new promotion
        /// </summary>
        /// <remarks>
        /// Creates a new promotion that can be applied to game purchases.
        ///
        /// Business rules:
        /// - Only users with the <b>Admin</b> role can create promotions
        /// - Promotion dates must be valid
        /// - Discount value must be greater than zero
        /// </remarks>
        /// <param name="model">Promotion creation data</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <response code="200">Promotion created successfully</response>
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
        public async Task<IActionResult> CreatePromotion(
            [FromBody] RegisterPromotionCommand model,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError($"CreatePromotion failed: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Purchases a promotion
        /// </summary>
        /// <remarks>
        /// Applies an active promotion to a user's purchase.
        ///
        /// Business rules:
        /// - The user must be authenticated
        /// - The promotion must be active
        /// - The user must meet promotion requirements
        /// </remarks>
        /// <param name="model">Promotion purchase data</param>
        /// <param name="cancellationToken">Request cancellation token</param>
        /// <response code="200">Promotion purchased successfully</response>
        /// <response code="400">Validation or business rule error</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("Purchase")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PurchasePromotion(
            [FromBody] PurchasePromotionRequest body,
            CancellationToken cancellationToken)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogError("PurchasePromotion failed: User ID claim is missing or invalid.");
                return Unauthorized("User ID claim is missing or invalid.");
            }

            var model = new PurchasePromotionCommand(body.PromotionId, userId);
            var result = await _mediator.Send(model, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError($"PurchasePromotion failed: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        public record PurchasePromotionRequest(
        [Required(ErrorMessage = "Promotion id is required")]
        Guid PromotionId) : IRequest<ResultViewModel<List<GameLicense>>>
        {

        }
    }
}
