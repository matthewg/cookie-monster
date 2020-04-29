using CookieMonster.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CookieMonster.Ledger {
    public class Amounts : IEnumerable {
        public Amounts() {
            this._amounts = new Dictionary<Cookie, decimal>();
        }

        public void Add(Cookie currency, decimal amount) {
            if (currency.Equals(Cookie.CASH) && (Math.Abs(amount % 1) > 0)) {
                throw new ArgumentException($"Must transact integer amount of non-cash currency; tried to do {amount} of {currency.Name}");
            }

            _amounts[currency] = this.Get(currency) + amount;
            if (_amounts[currency] == 0.00M) {
                _amounts.Remove(currency);
            }
        }

        public void Subtract(Cookie currency, decimal amount) {
            this.Add(currency, 0.00M - amount);
        }

        public decimal Get(Cookie currency) {
            decimal currentAmount;
            _amounts.TryGetValue(currency, out currentAmount);
            return currentAmount;
        }

        public int Count { get => _amounts.Count; }

        private Dictionary<Cookie, decimal> _amounts;

        IEnumerator IEnumerable.GetEnumerator() {
            return _amounts.GetEnumerator();
        }

        public Dictionary<Cookie, decimal>.Enumerator GetEnumerator() {
            return _amounts.GetEnumerator();
        }
    }
}