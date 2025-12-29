namespace FiapCloudGames.Domain.Identity
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
