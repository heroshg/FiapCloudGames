using FiapCloudGames.Application.Commands.RegisterUser;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.UnitTests.Common;
using Moq;
using Xunit;

namespace FiapCloudGames.UnitTests.Application.Commands;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task ValidCommand_Handle_ReturnsSuccessAndCreatesUser()
    {
        // Arrange
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

        var validPassword = PasswordFakers.GenerateValidPassword();

        var cmd = new RegisterUserCommand(
            Name: "Test User",
            Email: "user@example.com",
            Password: validPassword
        );

        // Act
        var result = await sut.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, result.Data);

        users.Verify(r => r.IsEmailRegisteredAsync("user@example.com"), Times.Once);
        hasher.Verify(h => h.HashPassword(validPassword), Times.Once);

        users.Verify(r => r.AddUserAsync(
            It.Is<User>(u =>
                u.Email.Address == "user@example.com" &&
                u.Name == "Test User" &&
                u.Role == Role.User &&
                u.Password.Value == "argon2id.4.65536.2.salt.hash"),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task InvalidEmail_Handle_ThrowsDomainExceptionAndDoesNotCallDependencies()
    {
        // Arrange
        var users = new Mock<IUserRepository>();
        var hasher = new Mock<IPasswordHasher>();

        var uniqueness = new EmailUniquenessPolicy(users.Object);
        var sut = new RegisterUserCommandHandler(uniqueness, users.Object, hasher.Object);

        var validPassword = PasswordFakers.GenerateValidPassword();

        var cmd = new RegisterUserCommand(
            Name: "Test User",
            Email: "email-invalido",
            Password: validPassword
        );

        // Act
        var act = async () => await sut.Handle(cmd, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DomainException>(act);

        users.Verify(r => r.IsEmailRegisteredAsync(It.IsAny<string>()), Times.Never);
        hasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
        users.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task InvalidPassword_Handle_ThrowsDomainExceptionAndDoesNotCallDependencies()
    {
        // Arrange
        var users = new Mock<IUserRepository>();
        var hasher = new Mock<IPasswordHasher>();

        var uniqueness = new EmailUniquenessPolicy(users.Object);
        var sut = new RegisterUserCommandHandler(uniqueness, users.Object, hasher.Object);

        var invalidPassword = PasswordFakers.GenerateTooShortPassword();

        var cmd = new RegisterUserCommand(
            Name: "Test User",
            Email: "user@example.com",
            Password: invalidPassword
        );

        // Act
        var act = async () => await sut.Handle(cmd, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DomainException>(act);

        users.Verify(r => r.IsEmailRegisteredAsync(It.IsAny<string>()), Times.Never);
        hasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
        users.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task EmailAlreadyRegistered_Handle_ThrowsDomainExceptionAndDoesNotHashOrCreateUser()
    {
        // Arrange
        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync("user@example.com"))
            .ReturnsAsync(true);

        var hasher = new Mock<IPasswordHasher>();

        var uniqueness = new EmailUniquenessPolicy(users.Object);
        var sut = new RegisterUserCommandHandler(uniqueness, users.Object, hasher.Object);

        var validPassword = PasswordFakers.GenerateValidPassword();

        var cmd = new RegisterUserCommand(
            Name: "Test User",
            Email: "user@example.com",
            Password: validPassword
        );

        // Act
        var act = async () => await sut.Handle(cmd, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DomainException>(act);

        users.Verify(r => r.IsEmailRegisteredAsync("user@example.com"), Times.Once);
        hasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
        users.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
