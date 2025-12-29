namespace FiapCloudGames.Domain.Identity
{
    public interface IPasswordHasher
    {
        string HashPasswordAsync(string password);

        bool VerifyPasswordAsync(string hashedPassword, string providedPassword);
    }
}
