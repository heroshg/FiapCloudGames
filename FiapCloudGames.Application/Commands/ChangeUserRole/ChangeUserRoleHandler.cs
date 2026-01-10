using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Identity.Repositories;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.ChangeUserRole
{
    public class ChangeUserRoleHandler : IRequestHandler<ChangeUserRoleCommand, ResultViewModel<Guid>>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserRoleHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResultViewModel<Guid>> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if(user is null)
            {
                return ResultViewModel<Guid>.Error("User not found.");
            }

            user.ChangeRole(request.Role);
            await _userRepository.UpdateAsync(user, cancellationToken);
            return ResultViewModel<Guid>.Success(user.Id);
        }
    }
}
