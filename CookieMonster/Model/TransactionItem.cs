namespace CookieMonster.Model
{
    public struct TransactionItem {
        public TransactionItem(Cookie currency, decimal amount, ValueStore from, ValueStore to) {
            this.Currency = currency;
            this.Amount = amount;
            this.From = from;
            this.To = to;
        }

        Cookie Currency { get; }
        decimal Amount { get; }
        ValueStore From { get; }
        ValueStore To { get; }
    }
}
