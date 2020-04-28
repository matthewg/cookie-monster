using CookieMonster.Model;
using System.Collections;
using System.Collections.ObjectModel;

namespace CookieMonster.Storage {
    public class InMemoryStorage : IStorage {
        public InMemoryStorage() {
            this._booths = new BoothDictionary();
            this._cookies = new CookieDictionary();
            this._customers = new CustomerDictionary();
            this._scouts = new ScoutDictionary();
            this._transactions = new TransactionDictionary();
            this._valueStores = new ValueStoreDictionary();
        }

        public ReadOnlyCollection<Booth> Booths() {
            return new ReadOnlyCollection<Booth>(_booths);
        }
        public ReadOnlyCollection<Cookie> Cookies() {
            return new ReadOnlyCollection<Cookie>(_cookies);
        }
        public ReadOnlyCollection<Customer> Customers() {
            return new ReadOnlyCollection<Customer>(_customers);
        }
        public ReadOnlyCollection<Scout> Scouts() {
            return new ReadOnlyCollection<Scout>(_scouts);
        }
        public ReadOnlyCollection<Transaction> Transactions() {
            return new ReadOnlyCollection<Transaction>(_transactions);
        }
        public ReadOnlyCollection<ValueStore> ValueStores() {
            return new ReadOnlyCollection<ValueStore>(_valueStores);
        }

        public void DeleteBooth(Booth booth) {
            _booths.Remove(booth);
        }
        public void DeleteCookie(Cookie cookie) {
            _cookies.Remove(cookie);
        }
        public void DeleteCustomer(Customer customer) {
            _customers.Remove(customer);
        }
        public void DeleteScout(Scout scout) {
            _scouts.Remove(scout);
        }
        public void DeleteTransaction(Transaction transaction) {
            _transactions.Remove(transaction);
        }
        public void DeleteValueStore(ValueStore valueStore) {
            _valueStores.Remove(valueStore);
        }

        public void PutBooth(Booth booth) {
            _booths.Add(new Booth(booth.ValueStore, booth.Location, booth.Notes, booth.ShiftTime, booth.Scouts));
        }
        public void PutCookie(Cookie cookie) {
            _cookies.Add(cookie);
        }
        public void PutCustomer(Customer customer) {
            _customers.Add(new Customer(customer.ValueStore, customer.Address, customer.PhoneNumber, customer.Email, customer.Notes));
        }
        public void PutScout(Scout scout) {
            _scouts.Add(new Scout(scout.ValueStore, scout.DigitalCookieUrl));
        }
        public void PutTransaction(Transaction transaction) {
            _transactions.Add(transaction);
        }
        public void PutValueStore(ValueStore valueStore) {
            _valueStores.Add(valueStore);
        }

        private class BoothDictionary : KeyedCollection<ulong, Booth> {
            protected override ulong GetKeyForItem(Booth item) {
                return item.ValueStore.Id;
            }
        }
        private class CookieDictionary : KeyedCollection<ulong, Cookie> {
            protected override ulong GetKeyForItem(Cookie item) {
                return item.Id;
            }
        }
        private class CustomerDictionary : KeyedCollection<ulong, Customer> {
            protected override ulong GetKeyForItem(Customer item) {
                return item.ValueStore.Id;
            }
        }
        private class ScoutDictionary : KeyedCollection<ulong, Scout> {
            protected override ulong GetKeyForItem(Scout item) {
                return item.ValueStore.Id;
            }
        }
        private class TransactionDictionary : KeyedCollection<ulong, Transaction> {
            protected override ulong GetKeyForItem(Transaction item) {
                return item.Id;
            }
        }
        private class ValueStoreDictionary : KeyedCollection<ulong, ValueStore> {
            protected override ulong GetKeyForItem(ValueStore item) {
                return item.Id;
            }
        }
        private BoothDictionary _booths;
        private CookieDictionary _cookies;
        private CustomerDictionary _customers;
        private ScoutDictionary _scouts;
        private TransactionDictionary _transactions;
        private ValueStoreDictionary _valueStores;
    }
}
