namespace CookieMonster.Model
{
    public struct TransactionItem {
        public TransactionItem(Cookie currency, decimal amount, ValueStore from, ValueStore to) {
            this.Currency = currency;
            this.Amount = amount;
            this.From = from;
            this.To = to;
        }

        public readonly Cookie Currency;
        public readonly decimal Amount;
        public readonly ValueStore From;
        public readonly ValueStore To;
    }
}
