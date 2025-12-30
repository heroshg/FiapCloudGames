using FiapCloudGames.Domain.Identity;
using Xunit;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class UserTests
{
    [Fact]
    public void Ctor_ShouldSetDefaults()
    {
        var email = new Email("user@example.com");
        var password = Password.FromHash("hash");

        var user = new User(email, password);

        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.Equal(Role.User, user.Role);
        Assert.Equal("user@example.com", user.Username);
    }

    [Fact]
    public void TurnAdmin_ShouldSetRoleAndUpdateTimestamp()
    {
        var user = new User(new Email("user@example.com"), Password.FromHash("hash"));
        var before = user.UpdatedAt;

        user.TurnAdmin();

        Assert.Equal(Role.Admin, user.Role);
        Assert.True(user.UpdatedAt >= before);
    }
}
