using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.GameAggregate;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.GetAllGames
{
    public class GetAllGamesHandler : IRequestHandler<GetAllGamesQuery, ResultViewModel<List<GameViewModel>>>
    {
        private readonly IGameRepository _repository;

        public GetAllGamesHandler(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<List<GameViewModel>>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {
            var games = await _repository.GetAllAsync(request.Name, request.Page, request.PageSize, cancellationToken);

            List<GameViewModel> gameViewModels = [];

            foreach (var game in games)
            {
                gameViewModels.Add(GameViewModel.FromEntity(game));
            }

            return ResultViewModel<List<GameViewModel>>.Success(gameViewModels);
        }
    }
}
