using FiapCloudGames.Application.Commands.PurchasePromotion;
using FiapCloudGames.Application.Commands.RegisterPromotion;
using FiapCloudGames.Infrastructure.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PromotionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly BaseLogger<GameLicensesController> _logger;

        public PromotionsController(IMediator mediator, BaseLogger<GameLicensesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreatePromotion(RegisterPromotionCommand model, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(model, cancellationToken);

            if(!result.IsSuccess)
            {
                _logger.LogError($"CreatePromotion failed: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("Purchase")]
        public async Task<IActionResult> PurchasePromotion(PurchasePromotionCommand model, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);
            if(!result.IsSuccess)
            {
                _logger.LogError($"PurchasePromotion failed: {result.Message}");
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

    }
}
