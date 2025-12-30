using FiapCloudGames.Application.Commands.RegisterUser;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using Moq;
using Xunit;

namespace FiapCloudGames.UnitTests.Application.Commands;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsRegistered()
    {
        var expectedId = Guid.NewGuid();

        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync("user@example.com"))
            .ReturnsAsync(false);
        users.Setup(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var hasher = new Mock<IPasswordHasher>();
        hasher.Setup(h => h.HashPassword(It.IsAny<string>()))
            .Returns("argon2id.4.65536.2.salt.hash");

        var uniqueness = new EmailUniquenessPolicy(users.Object);
        var sut = new RegisterUserCommandHandler(uniqueness, users.Object, hasher.Object);

        var cmd = new RegisterUserCommand(
            Email: "user@example.com",
            Password: "Abcdef1!"
        );

        var result = await sut.Handle(cmd, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, result.Data);

        users.Verify(r => r.IsEmailRegisteredAsync("user@example.com"), Times.Once);
        hasher.Verify(h => h.HashPassword("Abcdef1!"), Times.Once);
        users.Verify(r => r.AddUserAsync(
            It.Is<User>(u =>
                u.Email.Address == "user@example.com" &&
                u.Username == "user@example.com" &&
                u.Role == Role.User &&
                u.Password.Value == "argon2id.4.65536.2.salt.hash"),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenEmailIsInvalid()
    {
        var users = new Mock<IUserRepository>();
        var hasher = new Mock<IPasswordHasher>();
        var uniqueness = new EmailUniquenessPolicy(users.Object);
        var sut = new RegisterUserCommandHandler(uniqueness, users.Object, hasher.Object);

        var cmd = new RegisterUserCommand(
            Email: "email-invalido",
            Password: "Abcdef1!"
        );

        await Assert.ThrowsAsync<DomainException>(() => sut.Handle(cmd, CancellationToken.None));

        users.Verify(r => r.IsEmailRegisteredAsync(It.IsAny<string>()), Times.Never);
        hasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
        users.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenPasswordIsInvalid()
    {
        var users = new Mock<IUserRepository>();
        var hasher = new Mock<IPasswordHasher>();
        var uniqueness = new EmailUniquenessPolicy(users.Object);
        var sut = new RegisterUserCommandHandler(uniqueness, users.Object, hasher.Object);

        var cmd = new RegisterUserCommand(
            Email: "user@example.com",
            Password: "123" // inválida
        );

        await Assert.ThrowsAsync<DomainException>(() => sut.Handle(cmd, CancellationToken.None));

        users.Verify(r => r.IsEmailRegisteredAsync(It.IsAny<string>()), Times.Never);
        hasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
        users.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenEmailIsAlreadyRegistered()
    {
        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync("user@example.com"))
            .ReturnsAsync(true);

        var hasher = new Mock<IPasswordHasher>();
        var uniqueness = new EmailUniquenessPolicy(users.Object);
        var sut = new RegisterUserCommandHandler(uniqueness, users.Object, hasher.Object);

        var cmd = new RegisterUserCommand(
            Email: "user@example.com",
            Password: "Abcdef1!"
        );

        await Assert.ThrowsAsync<DomainException>(() => sut.Handle(cmd, CancellationToken.None));

        users.Verify(r => r.IsEmailRegisteredAsync("user@example.com"), Times.Once);
        hasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
        users.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
