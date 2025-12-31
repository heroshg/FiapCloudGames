using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using Moq;
using Xunit;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class EmailUniquenessPolicyTests
{
    [Fact]
    public async Task GivenEmailAlreadyRegistered_WhenEnsureEmailIsUniqueAsync_ThenThrowsDomainException()
    {
        // Arrange
        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync("exists@example.com"))
            .ReturnsAsync(true);

        var sut = new EmailUniquenessPolicy(users.Object);

        // Act
        var act = async () => await sut.EnsureEmailIsUniqueAsync("exists@example.com");

        // Assert
        await Assert.ThrowsAsync<DomainException>(act);
        users.Verify(r => r.IsEmailRegisteredAsync("exists@example.com"), Times.Once);
    }

    [Fact]
    public async Task GivenEmailNotRegistered_WhenEnsureEmailIsUniqueAsync_ThenDoesNotThrow()
    {
        // Arrange
        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync("new@example.com"))
            .ReturnsAsync(false);

        var sut = new EmailUniquenessPolicy(users.Object);

        // Act
        var act = async () => await sut.EnsureEmailIsUniqueAsync("new@example.com");

        // Assert
        var ex = await Record.ExceptionAsync(act);
        Assert.Null(ex);

        users.Verify(r => r.IsEmailRegisteredAsync("new@example.com"), Times.Once);
    }
}
