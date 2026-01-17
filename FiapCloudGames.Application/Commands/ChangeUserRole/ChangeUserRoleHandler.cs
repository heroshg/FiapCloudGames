using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity.ValueObjects;
using FiapCloudGames.Domain.UserAggregate;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.ChangeUserRole
{
    public class ChangeUserRoleHandler : IRequestHandler<ChangeUserRoleCommand, ResultViewModel<UserAdminViewModel>>
    {
        private readonly IUserRepository _repository;

        public ChangeUserRoleHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<UserAdminViewModel>> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {

            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (user is null)
                return ResultViewModel<UserAdminViewModel>.Error("User not found.");

            user.ChangeRole(request.Role);

            await _repository.UpdateAsync(user, cancellationToken);

            return ResultViewModel<UserAdminViewModel>.Success(UserAdminViewModel.FromEntity(user));
        }
    }
}
