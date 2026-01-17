using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.GameAggregate;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterGame
{
    public class RegisterGameHandler : IRequestHandler<RegisterGameCommand, ResultViewModel<Guid>>
    {
        private readonly IGameRepository _repository;

        public RegisterGameHandler(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<Guid>> Handle(RegisterGameCommand request, CancellationToken cancellationToken)
        {
            var game = new Game(request.Name, request.Description, request.Price);
            var id = await _repository.AddGameAsync(game, cancellationToken);

            return ResultViewModel<Guid>.Success(id);
        }
    }
}
