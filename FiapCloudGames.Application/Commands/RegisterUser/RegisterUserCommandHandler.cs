using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Identity;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResultViewModel<Guid>>
    { 
        private readonly UniquenessChecker _uniquenessChecker;
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(UniquenessChecker uniquenessChecker, IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _uniquenessChecker = uniquenessChecker;
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultViewModel<Guid>> Handle(
            RegisterUserCommand request, 
            CancellationToken cancellationToken)
        {
            var email = new Email(request.Email);
            var plainPassword = Password.FromPlainText(request.Password);

            await _uniquenessChecker.EnsureEmailIsUniqueAsync(email.Address);

            var passwordHash = _passwordHasher.HashPassword(plainPassword.Value);

            var user = new User(
                email,
                Password.FromHash(passwordHash)
            );

            var id = await _repository.AddUserAsync(user, cancellationToken);

            return ResultViewModel<Guid>.Success(id);
        }
    }
}
