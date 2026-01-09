using FiapCloudGames.Application.Commands.RegisterGame;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Games;
using FiapCloudGames.UnitTests.Common;
using Moq;
using Xunit;

namespace FiapCloudGames.UnitTests.Application.Commands;

public class RegisterGameCommandHandlerTests
{

    [Fact]
    public async Task ValidCommand_Handle_CreatesGameAndReturnsSuccess()
    {
        // Arrange
        var expectedId = Guid.NewGuid();

        var repo = new Mock<IGameRepository>();
        repo.Setup(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var sut = new RegisterGameHandler(repo.Object);

        var cmd = new RegisterGameCommand(
            Name: "Hades",
            Description: "Roguelike",
            Price: 59.9m
        );

        // Act
        var result = await sut.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, result.Data);

        repo.Verify(r => r.AddGameAsync(
            It.Is<Game>(g =>
                g.Name == "Hades" &&
                g.Description == "Roguelike" &&
                g.Price == 59.9m),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RepositoryThrowsException_Handle_ThrowsSameException()
    {
        // Arrange
        var repo = new Mock<IGameRepository>();
        repo.Setup(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        var sut = new RegisterGameHandler(repo.Object);

        var cmd = new RegisterGameCommand(
            Name: "Hades",
            Description: "Roguelike",
            Price: 59.9m
        );

        // Act
        var act = async () => await sut.Handle(cmd, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("Database error", ex.Message);
    }

    [Fact]
    public async Task NegativePrice_Handle_ThrowsDomainExceptionAndDoesNotCallRepository()
    {
        // Arrange
        var repo = new Mock<IGameRepository>();
        var sut = new RegisterGameHandler(repo.Object);

        var cmd = new RegisterGameCommand(
            Name: "Hades",
            Description: "Roguelike",
            Price: -10m
        );

        // Act
        var act = async () => await sut.Handle(cmd, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<DomainException>(act);
        Assert.Equal("Game price cannot be negative.", ex.Message);

        repo.Verify(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [MemberData(
        nameof(PasswordFakers.NullOrWhiteSpaceStrings),
        MemberType = typeof(PasswordFakers)
    )]
    public async Task NullOrWhiteSpaceName_Handle_ThrowsDomainExceptionAndDoesNotCallRepository(string name)
    {
        // Arrange
        var repo = new Mock<IGameRepository>();
        var sut = new RegisterGameHandler(repo.Object);

        var cmd = new RegisterGameCommand(
            Name: name,
            Description: "Roguelike",
            Price: 59.9m
        );

        // Act
        var act = async () => await sut.Handle(cmd, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<DomainException>(act);
        Assert.Equal("Game name cannot be null or empty.", ex.Message);

        repo.Verify(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [MemberData(
        nameof(PasswordFakers.NullOrWhiteSpaceStrings),
        MemberType = typeof(PasswordFakers)
    )]
    public async Task NullOrWhiteSpaceDescription_Handle_ThrowsDomainExceptionAndDoesNotCallRepository(string description)
    {
        // Arrange
        var repo = new Mock<IGameRepository>();
        var sut = new RegisterGameHandler(repo.Object);

        var cmd = new RegisterGameCommand(
            Name: "Hades",
            Description: description,
            Price: 59.9m
        );

        // Act
        var act = async () => await sut.Handle(cmd, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<DomainException>(act);
        Assert.Equal("Game description cannot be null or empty.", ex.Message);

        repo.Verify(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CanceledToken_Handle_PropagatesOperationCanceledException()
    {
        // Arrange
        var repo = new Mock<IGameRepository>();
        repo.Setup(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .Returns<Game, CancellationToken>((_, ct) => Task.FromCanceled<Guid>(ct));

        var sut = new RegisterGameHandler(repo.Object);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var cmd = new RegisterGameCommand(
            Name: "Hades",
            Description: "Roguelike",
            Price: 59.9m
        );

        // Act
        var act = async () => await sut.Handle(cmd, cts.Token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(act);
    }
}
