using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Domain.Identity.ValueObjects;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ResultViewModel<Guid>>
    { 
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserSpecification _specification;

        public RegisterUserHandler(IUserRepository repository, IPasswordHasher passwordHasher, IUserSpecification specification)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _specification = specification;
        }

        public async Task<ResultViewModel<Guid>> Handle(
            RegisterUserCommand request, 
            CancellationToken cancellationToken)
        {
            var email = new Email(request.Email);
            var plainPassword = Password.FromPlainText(request.Password);

            var passwordHash = _passwordHasher.HashPassword(plainPassword.Value);

            if(!await _specification.IsSatisfiedByAsync(email, cancellationToken))
            {
                throw new DomainException("Email already in use."); 
            }

            var user = User.Create(
                request.Name,
                email,
                Password.FromHash(passwordHash)
            );

            var id = await _repository.AddAsync(user, cancellationToken);

            return ResultViewModel<Guid>.Success(id);
        }
    }
}
