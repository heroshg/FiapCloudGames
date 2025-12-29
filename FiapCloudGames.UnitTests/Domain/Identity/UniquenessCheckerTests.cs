using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using Moq;
using Xunit;

namespace FiapCloudGames.UnitTests.Domain.Identity;

public class UniquenessCheckerTests
{
    [Fact]
    public async Task EnsureEmailIsUniqueAsync_ShouldThrow_WhenEmailAlreadyRegistered()
    {
        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync("exists@example.com"))
            .ReturnsAsync(true);

        var sut = new EmailUniquenessPolicy(users.Object);

        await Assert.ThrowsAsync<DomainException>(() => sut.EnsureEmailIsUniqueAsync("exists@example.com"));
        users.Verify(r => r.IsEmailRegisteredAsync("exists@example.com"), Times.Once);
    }

    [Fact]
    public async Task EnsureEmailIsUniqueAsync_ShouldNotThrow_WhenEmailIsNotRegistered()
    {
        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync("new@example.com"))
            .ReturnsAsync(false);

        var sut = new EmailUniquenessPolicy(users.Object);

        await sut.EnsureEmailIsUniqueAsync("new@example.com");
        users.Verify(r => r.IsEmailRegisteredAsync("new@example.com"), Times.Once);
    }
}
