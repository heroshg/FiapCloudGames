using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Ctor_ShouldThrow_WhenEmpty(string? address)
    {
        Assert.Throws<DomainException>(() => new Email(address!));
    }

    [Theory]
    [InlineData("email-invalido")]
    [InlineData("a@")]
    [InlineData("@b.com")]
    [InlineData("a@b")]
    [InlineData("a b@c.com")]
    public void Ctor_ShouldThrow_WhenFormatInvalid(string address)
    {
        Assert.Throws<DomainException>(() => new Email(address));
    }

    [Fact]
    public void Ctor_ShouldSetAddress_WhenValid()
    {
        var email = new Email("test@example.com");

        Assert.Equal("test@example.com", email.Address);
    }
}
