enum CurrencyType {
    Cash,
    Cookie,
}

class Currency {
    public readonly CurrencyType CurrencyType;
    public readonly Cookie? Cookie;  // null for Cash currencies

    private Currency(CurrencyType currencyType, Cookie? cookie) {
        this.CurrencyType = currencyType;
        this.Cookie = cookie;
    }

    public static Currency Cash() {
        return new Currency(CurrencyType.Cash, null);
    }

    public static Currency Cookie(Cookie cookie) {
        return new Currency(CurrencyType.Cookie, cookie);
    }
}
