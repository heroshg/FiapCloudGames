using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.UnitTests.Common;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class PasswordTests
{

    [Theory]
    [MemberData(
        nameof(PasswordFakers.NullOrWhiteSpaceStrings),
        MemberType = typeof(PasswordFakers)
    )]
    public void GivenNullOrWhiteSpacePassword_WhenValidate_ThenThrowsDomainException(string? password)
    {
        // Arrange
        // (nothing else)

        // Act
        var act = () => PasswordPolicy.Validate(password!);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [MemberData(
        nameof(PasswordFakers.InvalidPasswordsForPolicy),
        MemberType = typeof(PasswordFakers)
    )]
    public void GivenInvalidPassword_WhenValidate_ThenThrowsDomainException(string password, string expectedMessagePart)
    {
        // Arrange
        // password already arranged by MemberData

        // Act
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate(password));

        // Assert
        Assert.Contains(expectedMessagePart, ex.Message);
    }

    [Fact]
    public void GivenValidPassword_WhenValidate_ThenDoesNotThrow()
    {
        // Arrange
        var valid = PasswordFakers.GenerateValidPassword();

        // Act
        var act = () => PasswordPolicy.Validate(valid);

        // Assert
        var ex = Record.Exception(act);
        Assert.Null(ex);
    }

    [Fact]
    public void GivenValidPlainTextPassword_WhenFromPlainText_ThenCreatesPasswordWithSameValue()
    {
        // Arrange
        var valid = PasswordFakers.GenerateValidPassword();

        // Act
        var password = Password.FromPlainText(valid);

        // Assert
        Assert.Equal(valid, password.Value);
    }

    [Theory]
    [MemberData(
        nameof(PasswordFakers.NullOrWhiteSpaceStrings),
        MemberType = typeof(PasswordFakers)
    )]
    public void GivenNullOrWhiteSpaceHash_WhenFromHash_ThenThrowsDomainException(string? hash)
    {
        // Arrange
        // (nothing else)

        // Act
        var act = () => Password.FromHash(hash!);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void GivenValidHash_WhenFromHash_ThenCreatesPasswordWithSameValue()
    {
        // Arrange
        var hash = "some-hash";

        // Act
        var password = Password.FromHash(hash);

        // Assert
        Assert.Equal(hash, password.Value);
    }
}
