using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Identity
{
    public class EmailUniquenessPolicy
    {
        private readonly IUserRepository _userRepository;

        public EmailUniquenessPolicy(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task EnsureEmailIsUniqueAsync(string email)
        {
            if( await _userRepository.IsEmailRegisteredAsync(email))
            {
                throw new DomainException("Email is already registered.");
            }
        }
    }
}
