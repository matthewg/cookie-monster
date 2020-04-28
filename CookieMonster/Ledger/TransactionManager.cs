using CookieMonster.Model;
using CookieMonster.Storage;

namespace CookieMonster.Ledger {

    public class InvalidTransaction : Exception {
        public InvalidTransaction(string message) : base(message) {}
    }
    public class TransactionManager {
        public TransactionManager(IStorage storage) {
            this._storage = storage;
        }

        public void OrderFromServiceUnitPantry(ValueStore cookieSource, ValueStore cookieDestination, ValueStore cashSource, List<MonetaryValue> order, string note) {
            if (cookieSource.ValueStoreType != ValueStoreType.Pantry) {
                throw new InvalidTransaction($"For SU pantry order, source '{cookieSource.Name}' must be pantry");
            }
            if (cookieDestination.ValueStoreType != ValueStoreType.Pantry) {
                throw new InvalidTransaction($"For SU pantry order, destination '{cookieDestination.Name}' must be pantry");
            }
            if (cashSource.ValueStoreType != ValueStoreType.BankAccount) {
                throw new InvalidTransaction($"For SU pantry order, cash source '${cashSource.Name}' must be bank account");
            }

            decimal cookieCostTotal;
            decimal providedCashTotal;
            Dictionary<Cookie, int> cookieOrderTotals;
            foreach (var orderItem in order) {
                if (orderItem.Currency == Cookie.CASH) {
                    providedCashTotal += orderItem.Amount;
                } else {
                    cookieCostTotal += orderItem.Currency.PricePerBox * orderItem.Amount;
                    cookieOrderTotals[orderItem.Currency] = cookieOrderTotals.TryGetValue(orderItem.Currency) + orderItem.Amount;
                }
            }

            if (cookieCostTotal != providedCashTotal) {
                throw new InvalidTransaction($"You ordered ${cookieCostTotal} worth of cookies from the SU pantry, but only paid ${providedCashTotal}.");
            }
            foreach (KeyValuePair<Cookie, int> cookieAmount in cookieOrderTotals) {
                if (cookieAmount.Value <= 0) {
                    throw new InvalidTransaction($"When ordering {cookieAmount.Key.Name}, you must order at least 1 box.");
                }
                if (cookieAmount.Value % cookieAmount.Key.BoxesPerCase != 0) {
                    throw new InvalidTransaction($"SU pantry orders must be in whole cases. You ordered {cookieAmount.Value} boxes of {cookieAmount.Key.Name}, which has {cookieAmount.Key.BoxesPerCase} boxes per case.");
                }
            }
            if (cookieOrderTotals.Count == 0) {
                throw new InvalidTransaction("You can't place an empty order from the SU pantry.");
            }

            // Order is valid, go ahead and add it to the ledger!

            var transactionItems = new List<TransactionItem>();
            // Bank account owes the SU pantry $X, and pays it.
            // TODO: Does this fulfillment happen immediately?
            transactionItems.Add(new TransactionItem(Cookie.CASH, cookieCostTotal, cashSource, cookieSource));
            transactionItems.Add(new TransactionItem(Cookie.CASH, 0.00M - cookieCostTotal, cookieSource, cashSource));

            foreach (KeyValuePair<Cookie, int> cookieAmount in cookieOrderTotals) {
                // SU pantry owes the troop pantry some cookies, and gives it to them.
                // TODO: Break this up: Order from pantry, and pick up from pantry.
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, cookieSource, cookieDestination));
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value * -1, cookieDestination, cookieSource));
            }

            // TODO: Shouldn't have to specify an ID here.
            // TODO: DateTime.Now?
            // TODO: MoneyType.Card?
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note, MoneyType.Card));
        }

        public void ReturnToServiceUnitPantry() {}

        public void TransferInventoryWithinTroop() {}        

        public void OpenBooth() {}

        public void CloseBooth() {}

        public void RecordPreSale() {}

        public void FulfillPreSale() {}

        public void SellDirect() {}

        public void SellDigitalCookie() {}

        public List<Transaction> UnfulfilledTransactions() {}

        public List<MonetaryValue> TotalIncome() {}

        public List<MonetaryValue> SalesForScout(Scout scout) {}

        public List<MonetaryValue> OutstandingInventory(ValueStore valueStore) {}

        private readonly IStorage _storage;
    }
}