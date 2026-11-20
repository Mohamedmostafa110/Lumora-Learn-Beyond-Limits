using Lumora.Models;

namespace Lumora.Services.Strategies
{
    public interface IDiscountStrategy
    {
        DiscountType SupportedType { get; }
        decimal CalculateDiscount(decimal originalPrice, decimal discountValue);
    }

    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        public DiscountType SupportedType => DiscountType.Percentage;

        public decimal CalculateDiscount(decimal originalPrice, decimal discountValue)
        {
            if (discountValue <= 0) return 0;
            var discount = (originalPrice * discountValue) / 100m;
            return Math.Min(originalPrice, discount);
        }
    }

    public class FixedAmountDiscountStrategy : IDiscountStrategy
    {
        public DiscountType SupportedType => DiscountType.FixedAmount;

        public decimal CalculateDiscount(decimal originalPrice, decimal discountValue)
        {
            if (discountValue <= 0) return 0;
            return Math.Min(originalPrice, discountValue);
        }
    }

    public interface IDiscountStrategyFactory
    {
        IDiscountStrategy GetStrategy(DiscountType discountType);
    }

    public class DiscountStrategyFactory : IDiscountStrategyFactory
    {
        private readonly IEnumerable<IDiscountStrategy> _strategies;

        public DiscountStrategyFactory(IEnumerable<IDiscountStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IDiscountStrategy GetStrategy(DiscountType discountType)
        {
            var strategy = _strategies.FirstOrDefault(s => s.SupportedType == discountType);
            return strategy ?? throw new NotSupportedException($"Discount type '{discountType}' has no configured strategy.");
        }
    }
}
