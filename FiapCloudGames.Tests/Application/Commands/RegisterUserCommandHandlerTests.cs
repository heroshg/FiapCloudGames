using FiapCloudGames.Application.Commands.RegisterUser;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Domain.Identity.ValueObjects;
using FiapCloudGames.Tests.Common;
using FluentAssertions;
using Moq;

namespace FiapCloudGames.Tests.Application.Commands;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task ValidCommand_Handle_ReturnsSuccessAndCreatesUser()
    {
        var expectedId = Guid.NewGuid();

        var users = new Mock<IUserRepository>();
        users.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var hasher = new Mock<IPasswordHasher>();
        hasher.Setup(h => h.HashPassword(It.IsAny<string>()))
            .Returns("argon2id.4.65536.2.salt.hash");

        var specification = new Mock<IUserSpecification>();
        specification
            .Setup(s => s.IsSatisfiedByAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = new RegisterUserHandler(
            users.Object,
            hasher.Object,
            specification.Object
        );

        var cmd = new RegisterUserCommand(
            Name: "Test User",
            Email: "user@example.com",
            Password: PasswordFakers.GenerateValidPassword()
        );

        // Act
        var result = await sut.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, result.Data);

        specification.Verify(
            s => s.IsSatisfiedByAsync(
                It.Is<Email>(e => e.Address == "user@example.com"),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        hasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Once);

        users.Verify(
            r => r.AddAsync(
                It.Is<User>(u =>
                    u.Email.Address == "user@example.com" &&
                    u.Name == "Test User" &&
                    u.Role.Value == Role.User.Value &&
                    u.Password.Value == "argon2id.4.65536.2.salt.hash"),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task InvalidEmail_Handle_ThrowsDomainExceptionAndDoesNotCallDependencies()
    {
        // Arrange
        var users = new Mock<IUserRepository>();
        var hasher = new Mock<IPasswordHasher>();
        var specification = new Mock<IUserSpecification>();

        var sut = new RegisterUserHandler(users.Object, hasher.Object, specification.Object);

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
        users.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task InvalidPassword_Handle_ThrowsDomainExceptionAndDoesNotCallDependencies()
    {
        // Arrange
        var users = new Mock<IUserRepository>();
        var hasher = new Mock<IPasswordHasher>();
        var specification = new Mock<IUserSpecification>();

        var sut = new RegisterUserHandler(users.Object, hasher.Object, specification.Object);

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
        users.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task EmailAlreadyRegistered_Handle_ThrowsDomainException_AndDoesNotAddUser()
    {
        var users = new Mock<IUserRepository>();
        users.Setup(r => r.IsEmailRegisteredAsync(It.IsAny<string>()))
             .ReturnsAsync(true);

        var hasher = new Mock<IPasswordHasher>();
        hasher.Setup(h => h.HashPassword(It.IsAny<string>()))
              .Returns("fake-hash");

        var specification = new Mock<IUserSpecification>();
        specification
            .Setup(s => s.IsSatisfiedByAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var sut = new RegisterUserHandler(
            users.Object,
            hasher.Object,
            specification.Object
        );

        var cmd = new RegisterUserCommand(
            Name: "Test User",
            Email: "user@example.com",
            Password: PasswordFakers.GenerateValidPassword()
        );

        // Act
        var ex = await Assert.ThrowsAsync<DomainException>(
            () => sut.Handle(cmd, CancellationToken.None)
        );

        // Assert
        ex.Message.Should().Be("Email already in use.");

        specification.Verify(
            s => s.IsSatisfiedByAsync(
                It.Is<Email>(e => e.Address == "user@example.com"),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        users.Verify(
            r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

}
