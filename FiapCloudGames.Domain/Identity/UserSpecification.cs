using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Domain.Identity.ValueObjects;

namespace FiapCloudGames.Domain.Identity
{
    public class UserSpecification : IUserSpecification
    {
        private readonly IUserRepository _repository;

        public UserSpecification(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsSatisfiedByAsync(Email email, CancellationToken ct)
        {
            return !await _repository.IsEmailRegisteredAsync(email.Address);
        }
    }
}
