using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.UnitTests.Common;
using Xunit;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class PasswordTests
{
    [Fact]
    public void FromPlainText_ShouldValidateAndCreatePassword()
    {
        var valid = PasswordGenerator.Generate(totalLength: 12, lettersCount: 7, digitsCount: 3, specialCount: 2);
        var password = Password.FromPlainText(valid);

        Assert.Equal(valid, password.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void FromHash_ShouldThrow_WhenEmpty(string? hash)
    {
        Assert.Throws<DomainException>(() => Password.FromHash(hash!));
    }

    [Fact]
    public void FromHash_ShouldCreatePassword_WhenValid()
    {
        var password = Password.FromHash("some-hash");

        Assert.Equal("some-hash", password.Value);
    }
}
