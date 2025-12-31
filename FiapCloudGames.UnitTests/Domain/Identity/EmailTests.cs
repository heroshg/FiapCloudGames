using System.Collections.Generic;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using Xunit;

namespace FiapCloudGames.UnitTests.Domain.Identity;

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
    public void GivenEmptyAddress_WhenCreatingEmail_ThenShouldThrowDomainException(string? address)
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
    public void GivenInvalidEmailFormat_WhenCreatingEmail_ThenShouldThrowDomainException(string address)
    {
        // Arrange
        // (address já vem do MemberData)

        // Act
        var act = () => new Email(address);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void GivenValidEmail_WhenCreatingEmail_ThenShouldSetAddress()
    {
        // Arrange
        var address = "test@example.com";

        // Act
        var email = new Email(address);

        // Assert
        Assert.Equal(address, email.Address);
    }
}
