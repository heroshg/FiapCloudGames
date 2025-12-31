using System;
using FiapCloudGames.Domain.Identity;
using Xunit;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class UserTests
{
    [Fact]
    public void GivenValidEmailAndPassword_WhenCreatingUser_ThenShouldSetDefaults()
    {
        // Arrange
        var email = new Email("user@example.com");
        var password = Password.FromHash("hash");

        // Act
        var user = new User(email, password);

        // Assert
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.Equal(Role.User, user.Role);
        Assert.Equal("user@example.com", user.Username);
    }

    [Fact]
    public void GivenUserRoleUser_WhenTurnAdmin_ThenShouldSetRoleAdminAndUpdateUpdatedAt()
    {
        // Arrange
        var user = new User(new Email("user@example.com"), Password.FromHash("hash"));
        var before = user.UpdatedAt;

        // Act
        user.TurnAdmin();

        // Assert
        Assert.Equal(Role.Admin, user.Role);
        Assert.True(user.UpdatedAt >= before);
    }
}
