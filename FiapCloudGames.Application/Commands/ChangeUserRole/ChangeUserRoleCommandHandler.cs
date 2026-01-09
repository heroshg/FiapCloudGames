using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Domain.Identity.ValueObjects;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.ChangeUserRole
{
    public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, ResultViewModel<UserAdminViewModel>>
    {
        private readonly IUserRepository _repository;

        public ChangeUserRoleCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<UserAdminViewModel>> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            Role role;
            try
            {
                role = Role.Parse(request.Role);
            }
            catch (DomainException ex)
            {
                return ResultViewModel<UserAdminViewModel>.Error(ex.Message);
            }

            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (user is null)
                return ResultViewModel<UserAdminViewModel>.Error("User not found.");

            user.ChangeRole(role);

            await _repository.UpdateAsync(user, cancellationToken);

            return ResultViewModel<UserAdminViewModel>.Success(UserAdminViewModel.FromEntity(user));
        }
    }
}
