using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.GameAggregate;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.GetAllGamesByUserId
{
    public class GetAllGamesByUserIdHandler : IRequestHandler<GetAllGamesByUserIdQuery, ResultViewModel<List<GameViewModel>>>
    {
        private readonly IGameRepository _repository;

        public GetAllGamesByUserIdHandler(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<List<GameViewModel>>> Handle(GetAllGamesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var games = await _repository.GetAllByUserIdAsync(request.UserId, cancellationToken);
            var gameViewModels = (await _repository
                                .GetAllByUserIdAsync(request.UserId, cancellationToken))
                                .Select(GameViewModel.FromEntity)
                                .ToList();

            return ResultViewModel<List<GameViewModel>>.Success(gameViewModels);
        }
    }
}
