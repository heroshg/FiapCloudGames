using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Identity;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResultViewModel<Guid>>
    { 
        private readonly EmailUniquenessPolicy _uniquenessPolicy;
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(EmailUniquenessPolicy uniquenessPolicy, IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _uniquenessPolicy = uniquenessPolicy;
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultViewModel<Guid>> Handle(
            RegisterUserCommand request, 
            CancellationToken cancellationToken)
        {
            var email = new Email(request.Email);
            var plainPassword = Password.FromPlainText(request.Password);

            await _uniquenessPolicy.EnsureEmailIsUniqueAsync(email.Address);

            var passwordHash = _passwordHasher.HashPassword(plainPassword.Value);

            var user = new User(
                request.Name,
                email,
                Password.FromHash(passwordHash)
            );

            var id = await _repository.AddUserAsync(user, cancellationToken);

            return ResultViewModel<Guid>.Success(id);
        }
    }
}
