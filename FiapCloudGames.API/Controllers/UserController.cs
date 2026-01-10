using FiapCloudGames.Application.Commands.ChangeUserRole;
using FiapCloudGames.Application.Commands.NewLogin;
using FiapCloudGames.Application.Commands.RegisterUser;
using FiapCloudGames.Infrastructure.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly BaseLogger<GameLicenseController> _logger;

        public UserController(IMediator mediator, BaseLogger<GameLicenseController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserCommand model, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError($"Register user failed: {result.Message}");
                return BadRequest(result.Message);
            }


            return Ok(result.Data);
        }
        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(NewLoginCommand model, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogError($"Login failed: {result.Message}");
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("ChangeRole")]
        public async Task<IActionResult> ChangeRole(ChangeUserRoleCommand model, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(model, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogError($"Change user role failed: {result.Message}");
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

    }
}
