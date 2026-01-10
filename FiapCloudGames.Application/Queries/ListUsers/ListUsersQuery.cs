using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.ListUsers
{
    public record ListUsersQuery(
        int Page = 1,
        int PageSize = 5,
        bool IncludeInactive = false
    ) : IRequest<ResultViewModel<PagedResultViewModel<UserAdminViewModel>>>;
}
