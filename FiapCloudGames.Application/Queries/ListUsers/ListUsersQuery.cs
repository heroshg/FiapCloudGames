using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.ListUsers
{
    public record ListUsersQuery(
       string? Search,
       bool IncludeInactive
   ) : IRequest<ResultViewModel<List<UserAdminViewModel>>>;
}
