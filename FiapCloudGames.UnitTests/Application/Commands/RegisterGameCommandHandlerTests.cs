using FiapCloudGames.Application.Commands.RegisterGame;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Games;
using Moq;

namespace FiapCloudGames.UnitTests.Application.Commands;

public class RegisterGameCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateGameAndReturnSuccess()
    {
        var expectedId = Guid.NewGuid();
        var repo = new Mock<IGameRepository>();
        repo.Setup(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var sut = new RegisterGameCommandHandler(repo.Object);

        var cmd = new RegisterGameCommand(
            Name: "Hades",
            Description: "Roguelike",
            Price: 59.9m
        );

        var result = await sut.Handle(cmd, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, result.Data);

        repo.Verify(r => r.AddGameAsync(
            It.Is<Game>(g => g.Name == "Hades" && g.Description == "Roguelike" && g.Price == 59.9m),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldThrow_WhenRepositoryThrowsException()
    {
        var repo = new Mock<IGameRepository>();
        repo.Setup(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        var sut = new RegisterGameCommandHandler(repo.Object);

        var cmd = new RegisterGameCommand("Hades", "Roguelike", 59.9m);

        var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(cmd, CancellationToken.None));
        Assert.Equal("Database error", ex.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowDomainException_WhenPriceIsNegative()
    {
        var repo = new Mock<IGameRepository>();
        var sut = new RegisterGameCommandHandler(repo.Object);

        var cmd = new RegisterGameCommand("Hades", "Roguelike", -10m);

        var ex = await Assert.ThrowsAsync<DomainException>(() => sut.Handle(cmd, CancellationToken.None));
        Assert.Equal("Game price cannot be negative.", ex.Message);

        repo.Verify(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNameIsEmpty()
    {
        var repo = new Mock<IGameRepository>();
        var sut = new RegisterGameCommandHandler(repo.Object);
        var cmd = new RegisterGameCommand(
            Name: "",
            Description: "Roguelike",
            Price: 59.9m
        );
        var ex = await Assert.ThrowsAsync<DomainException>(() => sut.Handle(cmd, CancellationToken.None));
        Assert.Equal("Game name cannot be null or empty.", ex.Message);

        repo.Verify(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNameIsNull()
    {
        var repo = new Mock<IGameRepository>();
        var sut = new RegisterGameCommandHandler(repo.Object);
        var cmd = new RegisterGameCommand(
            Name: null!,
            Description: "Roguelike",
            Price: 59.9m
        );
        var ex = await Assert.ThrowsAsync<DomainException>(() => sut.Handle(cmd, CancellationToken.None));
        Assert.Equal("Game name cannot be null or empty.", ex.Message);

        repo.Verify(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDescriptionIsEmpty()
    {
        var repo = new Mock<IGameRepository>();
        var sut = new RegisterGameCommandHandler(repo.Object);
        var cmd = new RegisterGameCommand(
            Name: "Hades",
            Description: "",
            Price: 59.9m
        );
        var ex = await Assert.ThrowsAsync<DomainException>(() => sut.Handle(cmd, CancellationToken.None));
        Assert.Equal("Game description cannot be null or empty.", ex.Message);

        repo.Verify(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDescriptionIsNull() {         var repo = new Mock<IGameRepository>();
        var sut = new RegisterGameCommandHandler(repo.Object);
        var cmd = new RegisterGameCommand(
            Name: "Hades",
            Description: null!,
            Price: 59.9m
        );
        var ex = await Assert.ThrowsAsync<DomainException>(() => sut.Handle(cmd, CancellationToken.None));
        Assert.Equal("Game description cannot be null or empty.", ex.Message);
        repo.Verify(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldPropagateCancellation_WhenRepositoryThrowsOperationCanceled()
    {
        var repo = new Mock<IGameRepository>();

        repo.Setup(r => r.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .Returns<Game, CancellationToken>((g, ct) =>
                Task.FromCanceled<Guid>(ct));

        var sut = new RegisterGameCommandHandler(repo.Object);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            sut.Handle(new RegisterGameCommand("Hades", "Roguelike", 59.9m), cts.Token));
    }

}
