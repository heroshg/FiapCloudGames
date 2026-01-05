namespace FiapCloudGames.Infrastructure.Auth
{
    public interface IAuthService
    {
        string GenerateToken(string email, string role);
    }
}
