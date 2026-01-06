using FiapCloudGames.Application.Commands.RegisterPromotion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PromotionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PromotionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreatePromotion(RegisterPromotionCommand model, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);
            if(!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

    }
}
