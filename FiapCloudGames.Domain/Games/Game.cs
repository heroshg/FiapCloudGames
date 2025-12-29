using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Games
{
    public class Game : Entity
    {
        public Game(string name, string description, decimal price) : base()
        {
            Name = name ?? throw new DomainException("Game name cannot be null");
            Description = description ?? throw new DomainException("Game description cannot be null");
            if(price < 0)
            {
                throw new DomainException("Game price cannot be negative");
            }
            Price = price;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
    }
}
