using CookieMonster.Model;
using System.Collections;
using System.Collections.ObjectModel;

namespace CookieMonster.Storage {
    class InMemoryStorage : IStorage {
        public InMemoryStorage() {
            this._cookies = new CookieDictionary();
            this._valueStores = new ValueStoreDictionary();
            this._transactions = new TransactionDictionary();
        }

        ReadOnlyCollection<Cookie> IStorage.Cookies() {
            return new ReadOnlyCollection<Cookie>(_cookies);
        }
        ReadOnlyCollection<ValueStore> IStorage.ValueStores() {
            return new ReadOnlyCollection<ValueStore>(_valueStores);
        }
        ReadOnlyCollection<Transaction> IStorage.Transactions() {
            return new ReadOnlyCollection<Transaction>(_transactions);
        }

        void IStorage.DeleteCookie(Cookie cookie) {
            _cookies.Remove(cookie);
        }
        void IStorage.DeleteValueStore(ValueStore valueStore) {
            _valueStores.Remove(valueStore);
        }
        void IStorage.DeleteTransaction(Transaction transaction) {
            _transactions.Remove(transaction);
        }

        void IStorage.PutCookie(Cookie cookie) {
            _cookies.Add(cookie);
        }
        void IStorage.PutValueStore(ValueStore valueStore) {
            _valueStores.Add(valueStore);
        }
        void IStorage.PutTransaction(Transaction transaction) {
            _transactions.Add(transaction);
        }

        private class CookieDictionary : KeyedCollection<ulong, Cookie> {
            protected override ulong GetKeyForItem(Cookie item) {
                return item.Id;
            }
        }
        private class ValueStoreDictionary : KeyedCollection<ulong, ValueStore> {
            protected override ulong GetKeyForItem(ValueStore item) {
                return item.Id;
            }
        }
        private class TransactionDictionary : KeyedCollection<ulong, Transaction> {
            protected override ulong GetKeyForItem(Transaction item) {
                return item.Id;
            }
        }
        private CookieDictionary _cookies;
        private ValueStoreDictionary _valueStores;
        private TransactionDictionary _transactions;
    }
}
