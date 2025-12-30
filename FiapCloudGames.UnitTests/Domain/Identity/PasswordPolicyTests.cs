using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.UnitTests.Common;
using System.Security.Cryptography;

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
        // Intentionally invalid (too short). Generated to avoid hardcoded "password-like" strings.
        var tooShort = PasswordGenerator.Generate(totalLength: 5, lettersCount: 2, digitsCount: 1, specialCount: 2);

        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate(tooShort));
        Assert.Contains("at least 8", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoLetter()
    {
        // No letters: digits + specials only
        var noLetter = PasswordGenerator.Generate(totalLength: 10, lettersCount: 0, digitsCount: 8, specialCount: 2);

        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate(noLetter));
        Assert.Contains("letter", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoDigit()
    {
        // No digits: letters + specials only
        var noDigit = PasswordGenerator.Generate(totalLength: 10, lettersCount: 8, digitsCount: 0, specialCount: 2);

        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate(noDigit));
        Assert.Contains("digit", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenNoSpecialCharacter()
    {
        // No specials: letters + digits only
        var noSpecial = PasswordGenerator.Generate(totalLength: 10, lettersCount: 7, digitsCount: 3, specialCount: 0);

        var ex = Assert.Throws<DomainException>(() => PasswordPolicy.Validate(noSpecial));
        Assert.Contains("special", ex.Message);
    }

    [Fact]
    public void Validate_ShouldNotThrow_WhenValid()
    {
        // Valid password with letters, digits and specials (generated to avoid secret scanning false positives)
        var valid = PasswordGenerator.Generate(totalLength: 12, lettersCount: 7, digitsCount: 3, specialCount: 2);

        PasswordPolicy.Validate(valid);
    }    
}
