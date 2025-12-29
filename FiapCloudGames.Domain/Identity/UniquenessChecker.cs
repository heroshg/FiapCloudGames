using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Identity
{
    public class UniquenessChecker
    {
        private readonly IUserRepository _userRepository;

        public UniquenessChecker(IUserRepository userRepository)
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
