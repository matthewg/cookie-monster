struct Item {
    readonly Currency currency;
    readonly decimal Amount;
    readonly ValueStore From;
    readonly ValueStore To;
}

enum MoneyType {
    Card,
    Cash,
}

struct Transaction {
    readonly uint64 Id;
    readonly DateTime Time;
    readonly List<Item> Items;
    readonly string Note;
    readonly MoneyType MoneyType;
    Transaction LinkedTransaction;
}
