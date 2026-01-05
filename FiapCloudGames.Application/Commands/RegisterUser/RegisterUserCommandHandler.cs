using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Domain.Identity.ValueObjects;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResultViewModel<Guid>>
    { 
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultViewModel<Guid>> Handle(
            RegisterUserCommand request, 
            CancellationToken cancellationToken)
        {
            var email = new Email(request.Email);
            var plainPassword = Password.FromPlainText(request.Password);

            var passwordHash = _passwordHasher.HashPassword(plainPassword.Value);

            var user = User.Create(
                request.Name,
                email,
                Password.FromHash(passwordHash),
                await _repository.IsEmailRegisteredAsync(request.Email)
            );

            var id = await _repository.AddAsync(user, cancellationToken);

            return ResultViewModel<Guid>.Success(id);
        }
    }
}
