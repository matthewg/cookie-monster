class InMemoryStorage : IStorage {
    readonly List<Cookie> IStorage.Cookies() {
        return _cookies;
    }
    readonly List<ValueStore> IStorage.ValueStores() {
        return _valueStores;
    }
    readonly List<Transaction> IStorage.Transactions() {
        return _transactions;
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

    private class CookieDictionary : KeyedCollection<uint64, Cookie> {
        protected override uint64 GetKeyForItem(Cookie item) {
            return item.Id;
        }
    }
    private class ValueStoreDictionary : KeyedCollection<uint64, ValueStore> {
        protected override uint64 GetKeyForItem(ValueStore item) {
            return item.Id;
        }
    }
    private class TransactionDictionary : KeyedCollection<uint64, Transaction> {
        protected override uint64 GetKeyForItem(Transaction item) {
            return item.Id;
        }
    }
    private CookieDictionary<uint64, Cookie> _cookies;
    private ValueStoreDictionary<uint64, ValueStore> _valueStores;
    private TransactionDictionary<uint64, Transaction> _transactions;
}