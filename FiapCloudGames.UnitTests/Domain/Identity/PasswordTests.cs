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
    public void NullOrWhiteSpacePassword_Validate_ThrowsDomainException(string? password)
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
    public void InvalidPassword_Validate_ThrowsDomainException(string password, string expectedMessagePart)
    {
        // Arrange
        // password already arranged by MemberData

        // Act
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate(password));

        // Assert
        Assert.Contains(expectedMessagePart, ex.Message);
    }

    [Fact]
    public void ValidPassword_Validate_DoesNotThrow()
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
    public void ValidPlainTextPassword_FromPlainText_CreatesPasswordWithSameValue()
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
    public void NullOrWhiteSpaceHash_FromHash_ThrowsDomainException(string? hash)
    {
        // Arrange
        // (nothing else)

        // Act
        var act = () => Password.FromHash(hash!);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void ValidHash_FromHash_CreatesPasswordWithSameValue()
    {
        // Arrange
        var hash = "some-hash";

        // Act
        var password = Password.FromHash(hash);

        // Assert
        Assert.Equal(hash, password.Value);
    }
}
