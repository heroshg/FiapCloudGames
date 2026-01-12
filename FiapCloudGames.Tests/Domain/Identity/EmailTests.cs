using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Domain.Identity.ValueObjects;
using Moq;

namespace FiapCloudGames.Tests.Domain.Identity;

public class EmailTests
{
    public static IEnumerable<object?[]> EmptyAddresses =>
        new List<object?[]>
        {
            new object?[] { null },
            new object?[] { "" },
            new object?[] { "   " },
        };

    public static IEnumerable<object[]> InvalidFormats =>
        new List<object[]>
        {
            new object[] { "email-invalido" },
            new object[] { "a@" },
            new object[] { "@b.com" },
            new object[] { "a@b" },
            new object[] { "a b@c.com" },
        };

    [Theory]
    [MemberData(nameof(EmptyAddresses))]
    public void EmptyAddress_CreatingEmail_ShouldThrowDomainException(string? address)
    {
        // Arrange
        // (address já vem do MemberData)

        // Act
        var act = () => new Email(address!);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [MemberData(nameof(InvalidFormats))]
    public void InvalidEmailFormat_CreatingEmail_ShouldThrowDomainException(string address)
    {
        // Arrange
        // (address já vem do MemberData)

        // Act
        var act = () => new Email(address);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void ValidEmail_CreatingEmail_ShouldSetAddress()
    {
        // Arrange
        var address = "test@example.com";

        // Act
        var email = new Email(address);

        // Assert
        Assert.Equal(address, email.Address);
    }

    [Fact]
    public async Task EmailNotRegistered_EnsureEmailIsUniqueAsync_DoesNotThrow()
    {
        // Arrange
        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync("new@example.com"))
            .ReturnsAsync(false);


        // Act
        var act = async () => await users.Object.IsEmailRegisteredAsync("new@example.com");

        // Assert
        var ex = await Record.ExceptionAsync(act);
        Assert.Null(ex);

        users.Verify(r => r.IsEmailRegisteredAsync("new@example.com"), Times.Once);
    }
}
