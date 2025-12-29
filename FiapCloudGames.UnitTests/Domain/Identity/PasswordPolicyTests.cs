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
        // Dummy short password (intentionally invalid)
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate("Tst1!"));
        Assert.Contains("at least 8", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoLetter()
    {
        // No letters: only digits/spaces/special chars
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate("0000 000!"));
        Assert.Contains("letter", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoDigit()
    {
        // No digits: letters + underscore + special
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate("NO_DIGITS!"));
        Assert.Contains("digit", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoSpecialCharacter()
    {
        // No special char: letters + digits only
        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate("TESTPASS1"));
        Assert.Contains("special", ex.Message);
    }

    [Fact]
    public void Validate_ShouldNotThrow_WhenValid()
    {
        // Explicit dummy value to avoid secret scanners false positives
        PasswordPolicy.Validate("DUMMY_TEST_9$");
    }
}
