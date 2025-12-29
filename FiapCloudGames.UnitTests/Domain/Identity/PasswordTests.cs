using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using Xunit;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class PasswordTests
{
    [Fact]
    public void FromPlainText_ShouldValidateAndCreatePassword()
    {
        var password = Password.FromPlainText("Abcdef1!");

        Assert.Equal("Abcdef1!", password.Value);
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
