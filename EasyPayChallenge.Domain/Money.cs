namespace EasyPayChallenge.Domain
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }

        public Money(decimal amount, Currency currency = Currency.USD)
        {
            Amount = amount;
            Currency = currency;
        }

        public override string ToString()
        {
            return $"{Currency} {Amount:F2}";
        }
    }
}