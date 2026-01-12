using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.Identity.ValueObjects;
using System;
using Xunit;

namespace FiapCloudGames.Tests.Domain.Identity;

public class UserTests
{
    public static IEnumerable<object[]> NullOrWhiteSpaceNames =>
    [
        new object[] { null },
        new object[] { "" },
        new object[] { "   " }
    ];

    [Fact]
    public void ValidEmailAndPassword_CreatingUser_ShouldSetDefaults()
    {
        // Arrange
        var name = "Test User";
        var email = new Email("user@example.com");
        var password = Password.FromHash("hash");

        // Act
        var user = User.Create(name, email, password);

        // Assert
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.Equal(Role.User.Value, user.Role.Value);
        Assert.Equal("Test User", user.Name);
    }

    [Fact]
    public void UserRoleUser_ChangeRoleToAdmin_ShouldSetRoleAdminAndUpdateUpdatedAt()
    {
        // Arrange
        var user = User.Create("Test User", new Email("user@example.com"), Password.FromHash("hash"));
        var before = user.UpdatedAt;

        // Act
        user.ChangeRole("Admin");

        // Assert
        Assert.Equal(Role.Admin.Value, user.Role.Value);
        Assert.True(user.UpdatedAt >= before);
    }

    [Theory]
    [MemberData(nameof(NullOrWhiteSpaceNames))]
    public void NullOrWhiteSpaceName_CreatingUser_ThrowsDomainException(string? name)
    {
        // Arrange
        var email = new Email("user@example.com");
        var password = Password.FromHash("hash");

        // Act
        var act = () => User.Create(name!, email, password);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void NameLongerThan150Characters_CreatingUser_ThrowsDomainException()
    {
        // Arrange
        var name = new string('A', 151);
        var email = new Email("user@example.com");
        var password = Password.FromHash("hash");

        // Act
        var act = () => User.Create(name, email, password);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void NameWith150Characters_CreatingUser_DoesNotThrow()
    {
        // Arrange
        var name = new string('A', 150);
        var email = new Email("user@example.com");
        var password = Password.FromHash("hash");

        // Act
        var act = () =>  User.Create(name, email, password);

        // Assert
        var ex = Record.Exception(act);
        Assert.Null(ex);
    }
}
