using CookieMonster.Model;
using CookieMonster.Storage;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CookieMonster.Ledger {

    public class InvalidTransaction : Exception {
        public InvalidTransaction(string message) : base(message) {}
    }
    public class TransactionManager {
        public TransactionManager(IStorage storage) {
            this._storage = storage;
            ReadOnlyCollection<ValueStore> valueStores = this._storage.ValueStores();
            if (valueStores.Count == 0) {
                this.InitializeValueStores();
                valueStores = this._storage.ValueStores();
            }

            bool foundCouncil = false;
            bool foundDigitalCookie = false;
            bool foundServiceUnit = false;
            bool foundTroop = false;
            bool foundVoid = false;
            foreach (var valueStore in valueStores) {
                switch (valueStores.ValueStoreType) {
                    case ValueStoreType.Council:
                      if (foundCouncil) {
                          throw new RuntimeException("Duplicate Council");
                      }
                      foundCouncil = true;
                      this._council = valueStore;
                      break;
                    case ValueStoreType.DigitalCookie:
                      if (foundDigitalCookie) {
                          throw new RuntimeException("Duplicate Digital Cookie");
                      }
                      foundDigitalCookie = true;
                      this._digitalCookie = valueStore;
                      break;
                    case ValueStoreType.ServiceUnit:
                      if (foundServiceUnit) {
                          throw new RuntimeException("Duplicate Service Unit");
                      }
                      foundServiceUnit = true;
                      this._serviceUnit = valueStore;
                      break;
                    case ValueStoreType.Troop:
                      if (foundTroop) {
                          throw new RuntimeException("Duplicate Troop");
                      }
                      foundTroop = true;
                      this._troop = valueStore;
                      break;
                    case ValueStoreType.Void:
                      if (foundVoid) {
                          throw new RuntimeException("Duplicate Void");
                      }
                      foundVoid = true;
                      this._void = valueStore;
                      break;
                    default:
                        break;
                }
            }
            if (!foundCouncil || !foundDigitalCookie || !foundServiceUnit || !foundTroop || !foundVoid) {
                throw new RuntimeException("Missing value stores");
            }
        }

        private void InitializeValueStores() {
            this._storage.PutValueStore(new ValueStore(ValueStoreType.Council, 0, "Council"));
            this._storage.PutValueStore(new ValueStore(ValueStoreType.DigitalCookie, 0, "Digital Cookie"));
            this._storage.PutValueStore(new ValueStore(ValueStoreType.ServiceUnit, 0, "Service Unit"));
            this._storage.PutValueStore(new ValueStore(ValueStoreType.Troop, 0, "Troop"));
            this._storage.PutValueStore(new ValueStore(ValueStoreType.Void, 0, "Void"));
        }

        public void OrderFromServiceUnitPantry(Dictionary<Cookie, int> order, string note) {
            this.validateServiceUnitOrder(order);

            var transactionItems = new List<TransactionItem>();
            // SU pantry owes troop some cookies.
            foreach (var cookieAmount in order) {
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value * -1, this._serviceUnit, this._troop);
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (ordered from pantry)"));
        }

        public void CancelServiceUnitPantryOrder(Dictionary<Cookie, int> order, string note) {
            this.validateServiceUnitOrder(order);
            Dictionary<Cookie, int> outstandingPantryCookies = this.outstandingServiceUnitPantryCookies();
            foreach (var cookieOrder in order) {
                if (cookieOrder.Value > outstandingPantryCookies.TryGetValue(cookieOrder.Key)) {
                    throw new InvalidTransaction($"{cookieOrder.Value} of {cookieOrder.Key.Name} is more than has been requested from SU pantry");
                }
            }

            var transactionItems = new List<TransactionItem>();
            // We no longer want cookies, or pantry can't provide them.
            foreach (var cookieAmount in order) {
                // Cancel out the pantry's cookie liability and then void out the cookies.
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, this._serviceUnit, this._troop);
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, this._troop, this._void);
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (voided pantry order)"));
        }

        public void PickUpServiceUnitPantryOrder(Dictionary<Cookie, int> order) {
            this.validateServiceUnitOrder(order);
            Dictionary<Cookie, int> outstandingPantryCookies = this.outstandingServiceUnitPantryCookies();
            foreach (var cookieOrder in order) {
                if (cookieOrder.Value > outstandingPantryCookies.TryGetValue(cookieOrder.Key)) {
                    throw new InvalidTransaction($"{cookieOrder.Value} of {cookieOrder.Key.Name} is more than has been requested from SU pantry");
                }
            }

            var transactionItems = new List<TransactionItem>();
            foreach (var cookieAmount in order) {
                // Troop gets the cookies and owes money to council.
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, this._serviceUnit, this._troop);
                transactionItems.Add(new TransactionItem(Cookie.CASH, cookieAmount.Key.PricePerBox * cookieAmount.Value, this._troop, this._council);
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (picked up pantry order)"));
        }

        public void ReturnToServiceUnitPantry(Dictionary<Cookie, int> order) {
            this.validateServiceUnitOrder(order);

            var transactionItems = new List<TransactionItem>();
            foreach (var cookieAmount in order) {
                // Troop gives up the cookies and no longer owes money to council.
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, this._troop, this._serviceUnit);
                transactionItems.Add(new TransactionItem(Cookie.CASH, cookieAmount.Key.PricePerBox * cookieAmount.Value * -1, this._troop, this._council);
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (picked up pantry order)"));
        }

        public void TransferInventoryWithinTroop(ValueStore from, ValueStore to, Dictionary<Cookie, int> assets) {
        } 

        public void OpenBooth() {}

        public void CloseBooth() {}

        public void RecordPreSale() {}

        public void FulfillPreSale() {}

        public void SellDirect() {}

        public void SellDigitalCookie() {}

        public List<Transaction> UnfulfilledTransactions(ValueStore valueStore) {}

        public List<MonetaryValue> OutstandingBalance(ValueStore valueStore) {}

        public List<MonetaryValue> OutstandingInventory(ValueStore valueStore) {}

        private Dictionary<Cookie, int> outstandingServiceUnitPantryCookies() {
            List<Transaction> outstandingTransactions = UnfulfilledTransactions(this._serviceUnit);
            var outstandingCookies = new Dictionary<Cookie, int>();
            foreach (var transaction in outstandingTransactions) {
                foreach (var item in transaction.Items) {
                    if (item.Currency == Cookie.CASH) {
                        continue;
                    }
                    outstandingCookies[item.Currency] = outstandingCookies.TryGetValue(item.Currency) + item.Amount*-1;
                }
            }
            return outstandingCookies;
        }

        private void validateServiceUnitOrder(Dictionary<Cookie, int> order) {
            if (order.ContainsKey(Cookie.CASH)) {
                throw new InvalidTransaction("Can't request cash from SU pantry");
            }
            foreach (KeyValuePair<Cookie, int> cookieAmount in order) {
                if (cookieAmount.Value <= 0) {
                    throw new InvalidTransaction($"When ordering {cookieAmount.Key.Name}, you must order at least 1 box.");
                }
                if (cookieAmount.Value % cookieAmount.Key.BoxesPerCase != 0) {
                    throw new InvalidTransaction($"SU pantry orders must be in whole cases. You ordered {cookieAmount.Value} boxes of {cookieAmount.Key.Name}, which has {cookieAmount.Key.BoxesPerCase} boxes per case.");
                }
            }
            if (order.Count == 0) {
                throw new InvalidTransaction("You can't place an empty order from the SU pantry.");
            }
        }

        private readonly IStorage _storage;
        private readonly ValueStore _council;
        private readonly ValueStore _digitalCookie;
        private readonly ValueStore _serviceUnit;
        private readonly ValueStore _troop;
        private readonly ValueStore _void;
    }
}