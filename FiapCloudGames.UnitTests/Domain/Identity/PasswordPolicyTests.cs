using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using Xunit;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class PasswordPolicyTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_ShouldThrow_WhenEmpty(string? password)
    {
        Assert.Throws<DomainException>(() => PasswordPolicy.Validate(password!));
    }

    [Fact]
    public void Validate_ShouldThrow_WhenTooShort()
    {
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate("Ab1!"));
        Assert.Contains("at least 8", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoLetter()
    {
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate("1234567!"));
        Assert.Contains("letter", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoDigit()
    {
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate("Abcdefg!"));
        Assert.Contains("digit", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoSpecialCharacter()
    {
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate("Abcdefg1"));
        Assert.Contains("special", ex.Message);
    }

    [Fact]
    public void Validate_ShouldNotThrow_WhenValid()
    {
        PasswordPolicy.Validate("Abcdef1!");
    }
}
