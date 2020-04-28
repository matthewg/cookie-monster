interface IStorage {
    readonly List<Cookie> Cookies();
    readonly List<ValueStore> ValueStores();
    readonly List<Transaction> Transactions();

    void DeleteCookie(Cookie cookie);
    void DeleteValueStore(ValueStore valueStore);
    void DeleteTransaction(Transaction transaction);

    void PutCookie(Cookie cookie);
    void PutValueStore(ValueStore valueStore);
    void PutTransaction(Transaction transaction);

    public readonly List<Currency> Currencies() {
        var currencies = new List<Currency>();
        currencies.Add(Currency.Cash());
        foreach (var cookie in Cookies()) {
            currencies.Add(Currency.Cookie(cookie));
        }
        return currencies;
    }
}