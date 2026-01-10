using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Infrastructure.Auth;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.NewLogin
{
    public class NewLoginHandler : IRequestHandler<NewLoginCommand, ResultViewModel>
    {
        private readonly IAuthService _service;
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public NewLoginHandler(IAuthService service, IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _service = service;
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultViewModel> Handle(NewLoginCommand request, CancellationToken cancellationToken)
        {

            var user = await _repository.GetUser(request.Email);
            if(user is null)
            {
                return ResultViewModel.Error("Invalid email or password");
            }

            if(!_passwordHasher.VerifyPassword(request.Password, user.Password.Value))
            {
                return ResultViewModel.Error("Invalid email or password");
            }

            if (!user.IsActive) {                
                return ResultViewModel.Error("User is inactive");
            }

            var token = _service.GenerateToken(request.Email, user.Role.Value);

            var loginViewModel = new LoginViewModel(token);

            return ResultViewModel<LoginViewModel>.Success(loginViewModel);

        }
    }
}
