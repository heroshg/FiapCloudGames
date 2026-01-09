using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Identity.Repositories;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.ListUsers
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, ResultViewModel<List<UserAdminViewModel>>>
    {
        private readonly IUserRepository _repository;

        public ListUsersQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<List<UserAdminViewModel>>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.ListAsync(
                request.Search,
                request.IncludeInactive,
                cancellationToken
            );

            var result = users.Select(UserAdminViewModel.FromEntity).ToList();

            return ResultViewModel<List<UserAdminViewModel>>.Success(result);
        }
    }
}
