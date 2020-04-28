using CookieMonster.Model;

namespace CookieMonster.Leder {
    public struct MonetaryValue {
        public MonetaryValue(Cookie currency, decimal amount) {
            this.Cookie = Cookie;
            this.Amount = amount;
        }

        public readonly Cookie Currency;
        public readonly decimal Amount;
    }
}