using FiapCloudGames.Application.Commands.ChangeUserRole;
using FiapCloudGames.Application.Commands.DeleteUser;
using FiapCloudGames.Application.Commands.NewLogin;
using FiapCloudGames.Application.Commands.RegisterUser;
using FiapCloudGames.Application.Commands.UpdateUser;
using FiapCloudGames.Application.Queries.GetUserById;
using FiapCloudGames.Application.Queries.ListUsers;
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
        [HttpPost("login")]
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



        [HttpGet("admin/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] ListUsersQuery model, CancellationToken ct)
        {
            var result = await _mediator.Send(model, ct);

            if (!result.IsSuccess)
            {
                _logger.LogError(
                    $"Failed to list users. Reason: {result.Message}"
                );

                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }


        [HttpGet("admin/users/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id), ct);
            if (!result.IsSuccess)
            {
                _logger.LogWarning(
                    $"User not found. UserId: {id}"
                );

                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPut("admin/users/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest body, CancellationToken ct)
        {
            var cmd = new UpdateUserCommand(id, body.Name, body.Email, body.IsActive);

            var result = await _mediator.Send(cmd, ct);
            if (!result.IsSuccess) { 
                _logger.LogError(
                    $"Failed to update user. UserId: {id}. Reason: {result.Message}"
                );
                return BadRequest(result.Message); 
            }
            return Ok(result.Data);
        }

        [HttpPut("admin/users/{id:guid}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeUserRoleRequest body, CancellationToken ct)
        {
            var cmd = new ChangeUserRoleCommand(id, body.Role);

            var result = await _mediator.Send(cmd, ct);

            if (!result.IsSuccess)
            {
                _logger.LogError(
                    $"Failed to change user role. UserId: {id}. Reason: {result.Message}"
                );
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpDelete("admin/users/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken ct)
        {
            var cmd = new DeleteUserCommand(id);

            var result = await _mediator.Send(cmd, ct);

            if (!result.IsSuccess) { 
                _logger.LogError(
                    $"Failed to delete user. UserId: {id}. Reason: {result.Message}"
                );
                return BadRequest(result.Message); 
            }
            return NoContent();
        }
    }
    public record ChangeUserRoleRequest(string Role);
    public record UpdateUserRequest(
        string? Name = null,
        string? Email = null,
        bool? IsActive = null
    );
}

