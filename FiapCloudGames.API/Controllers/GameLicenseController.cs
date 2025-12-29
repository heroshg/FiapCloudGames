using FiapCloudGames.Application.Commands.PurchaseGameLicense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameLicenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameLicenseController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> PurchaseGameLicense(PurchaseGameLicenseCommand model,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result.Data);
        }
    }
}
