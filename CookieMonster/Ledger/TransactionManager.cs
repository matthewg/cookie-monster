using CookieMonster.Model;
using CookieMonster.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Negative amounts are liabilities
// Positive amounts are tranfers of real assets
// So, from:X to:Y amt:ABC-10 indicates that Y owes X 10 of ABC
// When Y actually gives the ABC10 to X, that's recorded as from:Y to:X amt:ABC+10

using Amounts = System.Collections.Generic.Dictionary<CookieMonster.Model.Cookie, decimal>;

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
                switch (valueStore.ValueStoreType) {
                    case ValueStoreType.Council:
                      if (foundCouncil) {
                          throw new ArgumentException("Duplicate Council");
                      }
                      foundCouncil = true;
                      this._council = valueStore;
                      break;
                    case ValueStoreType.DigitalCookie:
                      if (foundDigitalCookie) {
                          throw new ArgumentException("Duplicate Digital Cookie");
                      }
                      foundDigitalCookie = true;
                      this._digitalCookie = valueStore;
                      break;
                    case ValueStoreType.ServiceUnit:
                      if (foundServiceUnit) {
                          throw new ArgumentException("Duplicate Service Unit");
                      }
                      foundServiceUnit = true;
                      this._serviceUnit = valueStore;
                      break;
                    case ValueStoreType.Troop:
                      if (foundTroop) {
                          throw new ArgumentException("Duplicate Troop");
                      }
                      foundTroop = true;
                      this._troop = valueStore;
                      break;
                    case ValueStoreType.Void:
                      if (foundVoid) {
                          throw new ArgumentException("Duplicate Void");
                      }
                      foundVoid = true;
                      this._void = valueStore;
                      break;
                    default:
                        break;
                }
            }
            if (!foundCouncil || !foundDigitalCookie || !foundServiceUnit || !foundTroop || !foundVoid) {
                throw new ArgumentException("Missing value stores");
            }
        }

        private void InitializeValueStores() {
            this._storage.PutValueStore(new ValueStore(ValueStoreType.Council, 0, "Council"));
            this._storage.PutValueStore(new ValueStore(ValueStoreType.DigitalCookie, 0, "Digital Cookie"));
            this._storage.PutValueStore(new ValueStore(ValueStoreType.ServiceUnit, 0, "Service Unit"));
            this._storage.PutValueStore(new ValueStore(ValueStoreType.Troop, 0, "Troop"));
            this._storage.PutValueStore(new ValueStore(ValueStoreType.Void, 0, "Void"));
        }

        public void OrderFromServiceUnitPantry(Amounts order, string note) {
            this.validateServiceUnitOrder(order);

            var transactionItems = new List<TransactionItem>();
            // SU pantry owes troop some cookies.
            foreach (var cookieAmount in order) {
                transactionItems.Add(new TransactionItem(cookieAmount.Key, -cookieAmount.Value, this._troop, this._serviceUnit));
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (ordered from pantry)"));
        }

        public void CancelServiceUnitPantryOrder(Amounts order, string note) {
            this.validateServiceUnitOrder(order);
            Amounts outstandingPantryCookies = this.OutstandingBalance(this._serviceUnit);
            foreach (var cookieAmount in order) {
                var outstandingCookieRequest = -outstandingPantryCookies.Get(cookieAmount.Key);
                if (cookieAmount.Value > outstandingCookieRequest) {
                    throw new InvalidTransaction($"{cookieAmount.Value} of {cookieAmount.Key.Name} is more than the {outstandingCookieRequest} that has been requested from SU pantry");
                }
            }

            var transactionItems = new List<TransactionItem>();
            // We no longer want cookies, or pantry can't provide them.
            foreach (var cookieAmount in order) {
                // Cancel out the pantry's cookie liability.
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, this._troop, this._serviceUnit));
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (voided pantry order)"));
        }

        public void PickUpServiceUnitPantryOrder(Amounts order, string note) {
            this.validateServiceUnitOrder(order);
            Amounts outstandingPantryCookies = this.OutstandingBalance(this._serviceUnit);

            var transactionItems = new List<TransactionItem>();
            foreach (var cookieAmount in order) {
                var outstandingCookieRequest = -outstandingPantryCookies.Get(cookieAmount.Key);
                if (cookieAmount.Value > outstandingCookieRequest) {
                    // We picked up more than we originally asked for.
                    // Add a "troop needs cookies from pantry" item so the books balance.
                    transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value - outstandingCookieRequest, this._troop, this._serviceUnit));
                }
                // Troop gets the cookies and owes money to council.
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, this._serviceUnit, this._troop));
                transactionItems.Add(new TransactionItem(Cookie.CASH, cookieAmount.Key.PricePerBox * cookieAmount.Value * -1, this._council, this._troop));
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (picked up pantry order)"));
        }

        public void ReturnToServiceUnitPantry(Amounts order, string note) {
            this.validateServiceUnitOrder(order);

            var transactionItems = new List<TransactionItem>();
            foreach (var cookieAmount in order) {
                // Troop gives up the cookies and no longer owes money to council.
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, this._troop, this._serviceUnit));
                transactionItems.Add(new TransactionItem(Cookie.CASH, cookieAmount.Key.PricePerBox * cookieAmount.Value, this._council, this._troop));
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (returned cookies to pantry)"));
        }

        public void TransferInventoryWithinTroop(ValueStore from, ValueStore to, Amounts assets, string note) {
            if ((from.ValueStoreType != ValueStoreType.Troop) &&
                (from.ValueStoreType != ValueStoreType.TroopSecondaryPantry)) {
                throw new ArgumentException($"Intra-troop transfer can't be used with source {from.Name} type {from.ValueStoreType}");
            }
            if ((to.ValueStoreType != ValueStoreType.Troop) &&
                (to.ValueStoreType != ValueStoreType.TroopSecondaryPantry)) {
                throw new ArgumentException($"Intra-troop transfer can't be used with destination {to.Name} type {to.ValueStoreType}");
            }
            this.transferAssets(from, to, assets, note + " (intra-troop transfer)");
        } 

        public void StockBooth(Booth booth, ValueStore stockSource, Amounts stock, string note) {
            if ((stockSource.ValueStoreType != ValueStoreType.Troop) &&
                (stockSource.ValueStoreType != ValueStoreType.TroopSecondaryPantry)) {
                throw new ArgumentException($"Can't stock booth from source {stockSource.Name} type {stockSource.ValueStoreType}");
            }
            this.transferAssets(stockSource, booth.ValueStore, stock, note + " (stocking booth)");
        }

        public void UnstockBooth(Booth booth, ValueStore stockDestination, Amounts stock, String note) {
            if ((stockDestination.ValueStoreType != ValueStoreType.Troop) &&
                (stockDestination.ValueStoreType != ValueStoreType.TroopSecondaryPantry)) {
                throw new ArgumentException($"Can't unstock booth to {stockDestination.Name} type {stockDestination.ValueStoreType}");
            }
            this.transferAssets(booth.ValueStore, stockDestination, stock, note + " (unstocking booth)");
        }

        public void CloseBooth(Booth booth) {
            var boothInventory = this.OutstandingBalance(booth.ValueStore);
            if (boothInventory.Count > 0) {
                throw new ArgumentException($"Can't close booth with non-empty inventory; have {boothInventory}");
            }
        }

        public void RecordPreSale(Scout scout, Customer customer, Amounts cookies, string note) {
            var transactionItems = new List<TransactionItem>();
            decimal moneyOwed = 0.00M;
            foreach (var cookieAmount in cookies) {
                if (cookieAmount.Key.Equals(Cookie.CASH)) {
                    throw new ArgumentException("Customers can't buy cash");
                }
                // Scout gets a cookie obligation, customer gets a money obligation
                transactionItems.Add(new TransactionItem(cookieAmount.Key, -cookieAmount.Value, customer.ValueStore, scout.ValueStore));
                moneyOwed += cookieAmount.Key.PricePerBox * cookieAmount.Value;
            }
            transactionItems.Add(new TransactionItem(Cookie.CASH, -moneyOwed, scout.ValueStore, customer.ValueStore));
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (pre-ordered cookies)"));
        }

        public void VoidPreSale(Scout scout, Customer customer, Amounts cookies, string note) {
            var outstandingPresale = this.OutstandingBalance(customer.ValueStore);
            var transactionItems = new List<TransactionItem>();
            decimal moneyNotOwed = 0.00M;
            foreach (var cookieAmount in cookies) {
                var amountPreordered = -outstandingPresale.Get(cookieAmount.Key);
                if (cookieAmount.Value > amountPreordered) {
                    throw new ArgumentException($"Can't void {cookieAmount.Value} of {cookieAmount.Key.Name} when customer {customer.ValueStore.Id} only ordered {amountPreordered}");
                }
                moneyNotOwed += cookieAmount.Key.PricePerBox * cookieAmount.Value;
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, customer.ValueStore, scout.ValueStore));
            }
            transactionItems.Add(new TransactionItem(Cookie.CASH, moneyNotOwed, scout.ValueStore, customer.ValueStore));
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (voided pre-order)"));
        }

        public void SellDirect(Scout scout, Customer customer, Amounts cookies, string note) {
            this.sellDirect(scout.ValueStore, customer, cookies, note);
        }
        public void SellDirect(Booth booth, Customer customer, Amounts cookies, string note) {
            this.sellDirect(booth.ValueStore, customer, cookies, note);
        }

        private void sellDirect(ValueStore seller, Customer customer, Amounts cookies, string note) {
            var outstandingPresale = this.OutstandingBalance(customer.ValueStore);
            var sellerInventory = this.OutstandingBalance(seller);
            var transactionItems = new List<TransactionItem>();
            decimal moneyCollected = 0.00M;
            decimal moneyNewlyOwed = 0.00M;
            foreach (var cookieAmount in cookies) {
                if (cookieAmount.Key.Equals(Cookie.CASH)) {
                    throw new ArgumentException("Can't sell cash");
                }
                if (cookieAmount.Value < 0.00M) {
                    throw new ArgumentException("Can't sell negative cookie amounts");
                }
                if ((cookieAmount.Value % 1) != 0.00M) {
                    throw new ArgumentException("Can't sell partial boxes of cookies");
                }
                
                if (sellerInventory.Get(cookieAmount.Key) > cookieAmount.Value) {
                    throw new ArgumentException($"{sellerInventory.Name} can't sell {cookieAmount.Value} or {cookieAmount.Key.Name}, they only have {sellerInventory.Get(cookieAmount.Key)}");
                }
                var amountPreordered = -outstandingPresale.Get(cookieAmount.Key);
                if (cookieAmount.Value > amountPreordered) {
                    // Maybe they never pre-ordered, or maybe they bought additional boxes at fulfill time.
                    // Either way, turns out they want some more cookies!
                    transactionItems.Add(new TransactionItem(cookieAmount.Key, -(cookieAmount.Value - amountPreordered), customer.ValueStore, scout.ValueStore));
                    moneyNewlyOwed += cookieAmount.Key.PricePerBox * (cookieAmount.Value - amountPreordered);
                }
                moneyCollected += cookieAmount.Key.PricePerBox * cookieAmount.Value;
                transactionItems.Add(new TransactionItem(cookieAmount.Key, cookieAmount.Value, seller, customer.ValueStore));
            }
            transactionItems.Add(new TransactionItem(Cookie.CASH, moneyCollected, customer.ValueStore, seller));
            if (moneyNewlyOwed > 0.00M) {
                transactionItems.Add(new TransactionItem(Cookie.CASH, -moneyNewlyOwed, seller, customer.ValueStore));
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note + " (sold cookies)"));
        }

        public void SellDigitalCookie(Scout scout, Customer customer, Amounts cookies) {
            var transactionItems = new List<TransactionItem>();
            decimal moneyCollected = 0.00M;
            foreach (var cookieAmount in cookies) {
                if (cookieAmount.Key.Equals(Cookie.CASH)) {
                    throw new ArgumentException("Can't sell cash");
                }
                if (cookieAmount.Value < 0.00M) {
                    throw new ArgumentException("Can't sell negative cookie amounts");
                }
                if ((cookieAmount.Value % 1) != 0.00M) {
                    throw new ArgumentException("Can't sell partial boxes of cookies");
                }
            }
        }

        // TODO: Need to separate concept of "inventory" from "net assets".
        // If you have 3 boxes and 3 preordered boxes, you do have *inventory* still.
        public Amounts OutstandingBalance(ValueStore valueStore) {
            var amounts = new Amounts();
            foreach (var item in _storage.TransactionItems(valueStore)) {
                if (item.To.Equals(valueStore)) {
                    amounts.Add(item.Currency, item.Amount);
                } else {
                    amounts.Subtract(item.Currency, item.Amount);
                }
            }
            return amounts;
        }

        private void validateServiceUnitOrder(Amounts order) {
            if (order.Get(Cookie.CASH) != 0.0M) {
                throw new InvalidTransaction("Can't exchange cash with SU pantry");
            }
            if (order.Get(Cookie.DONATION) != 0.0M) {
                throw new InvalidTransaction("Can't exchange donations with SU pantry");
            }
            foreach (var cookieAmount in order) {
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

        private void transferAssets(ValueStore from, ValueStore to, Amounts assets, string note) {
            var sourceInventory = this.OutstandingBalance(from);
            var transactionItems = new List<TransactionItem>();
            foreach (var asset in assets) {
                if (asset.Value <= 0M) {
                    throw new ArgumentException($"Can only transfer positive amounts of {asset.Key.Name}, tried to transfer {asset.Value} from {from.Name} to {to.Name}");
                }

                var stock = sourceInventory.Get(asset.Key);
                if (stock < asset.Value) {
                    throw new ArgumentException($"Can't transfer more than you have; {from.Name} has {stock} {asset.Key.Name}, tried to transfer {asset.Value} to {to.Name}");
                }

                transactionItems.Add(new TransactionItem(asset.Key, asset.Value, from, to));
                transactionItems.Add(new TransactionItem(asset.Key, -asset.Value, to, from));
            }
            _storage.PutTransaction(new Transaction(0, DateTime.Now, transactionItems, note));
        }

        private readonly IStorage _storage;
        private readonly ValueStore _council;
        private readonly ValueStore _digitalCookie;
        private readonly ValueStore _serviceUnit;
        private readonly ValueStore _troop;
        private readonly ValueStore _void;
    }
}