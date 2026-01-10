using FiapCloudGames.Application.Commands.PurchaseGameLicense;
using FiapCloudGames.Infrastructure.Logging;
using FiapCloudGames.Infrastructure.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameLicensesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly BaseLogger<GameLicensesController> _logger;

        public GameLicensesController(IMediator mediator, BaseLogger<GameLicensesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> PurchaseGameLicense(PurchaseGameLicenseCommand model,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError($"PurchaseGameLicense failed. Reason={result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
