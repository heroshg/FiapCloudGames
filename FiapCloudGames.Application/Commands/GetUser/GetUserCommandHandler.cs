using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Identity;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.GetUser
{
    public class GetUserCommandHandler
        : IRequestHandler<GetUserCommand, ResultViewModel<UserViewModel>>
    {
        private readonly IUserRepository _users;

        public GetUserCommandHandler(IUserRepository users)
        {
            _users = users;
        }

        public async Task<ResultViewModel<UserViewModel>> Handle(
            GetUserCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return ResultViewModel<UserViewModel>.Error("Invalid user id.");
            }

            var user = await _users.GetByIdAsync(request.Id, cancellationToken);

            if (user is null)
            {
                return ResultViewModel<UserViewModel>.Error("User not found.");
            }

            var vm = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Address,
                Role = user.Role
            };

            return ResultViewModel<UserViewModel>.Success(vm);
        }
    }
}
