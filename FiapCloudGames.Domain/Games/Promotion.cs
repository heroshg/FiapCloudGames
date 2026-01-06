using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Games
{
    public class Promotion : Entity
    {
        
        public Promotion Create(
            string name,
            Discount discount,
            DateTime? startsAt,
            DateTime? endsAt,
            List<Game> games)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Promotion name cannot be null or empty.", nameof(name));
            }
            Name = name;

            discount = discount ?? throw new ArgumentNullException(nameof(discount), "Discount cannot be null.");
            Discount = discount;

            if(startsAt is null)
            {
                throw new Exception("Promotion start date must be provided.");
            }
            if(startsAt >= endsAt)
            {
                throw new ArgumentException("Promotion start date must be earlier than end date.");
            }
            if(startsAt <= DateTime.UtcNow)
            {
                throw new ArgumentException("Promotion start date must be in the future.");
            }
            StartsAt = startsAt;

            if(endsAt is null)
            {
                throw new Exception("Promotion end date must be provided.");
            }
            if(endsAt <= startsAt)
            {
                throw new ArgumentException("Promotion end date must be later than start date.");
            }
            if(endsAt <= DateTime.UtcNow) {
                throw new ArgumentException("Promotion end date must be in the future.");
            }
            EndsAt = endsAt;

            if(games.Count == 0)
            {
                throw new ArgumentException("At least one game must be associated with the promotion.", nameof(games));
            }

            if(games.Any(g => g.IsActive))
            {
                throw new ArgumentException("All games associated with the promotion must be active.", nameof(games));
            }

            foreach(var game in games)
            {
                Games.Add(game);
            }


            IsActive = DateTime.UtcNow >= StartsAt && DateTime.UtcNow <= EndsAt;
            return this;
        }
        public string Name { get; private set; } = string.Empty;
        public Discount Discount { get; private set; } = null!;
        public DateTime? StartsAt { get; private set; }
        public DateTime? EndsAt { get; private set; }

        public ICollection<Game> Games => [];

        public decimal GetDiscountedPrice()
        {
            decimal originalPrice = Games.Sum(g => g.Price);
            if (Discount.Type == DiscountType.Percentage)
            {
                return originalPrice - (originalPrice * Discount.Value / 100);
            }
            if (Discount.Type == DiscountType.Fixed)
            {
                return originalPrice - Discount.Value;
            }

            throw new InvalidOperationException("Unknown discount type.");

        }


    }
}
