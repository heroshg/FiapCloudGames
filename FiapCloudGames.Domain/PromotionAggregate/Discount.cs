namespace FiapCloudGames.Domain.PromotionAggregate
{
    public class Discount
    {
        public Discount(decimal value, DiscountType type)
        {
            if (value <= 0 )
            {
                throw new ArgumentException("Discount value must be greater than zero.", nameof(value));
            }
            Value = value;
            Type = type;
        }

        public decimal Value { get; }
        public DiscountType Type { get; }

        public decimal Apply(decimal price)
        {
            return Type switch
            {
                DiscountType.Percentage => price - (price * Value / 100),
                DiscountType.Fixed => price - Value,
                _ => price
            };
        }
    }
}
