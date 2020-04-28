using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CookieMonster.Model
{
    public struct Transaction {
        public Transaction(ulong id,
                                 DateTime time,
                                 List<TransactionItem> items,
                                 string note,
                                 MoneyType moneyType) {
            this.Id = id;
            this.Time = time;
            this.Items = new List<TransactionItem>(items).AsReadOnly();
            this.Note = note;
            this.MoneyType = moneyType;
        }

        public readonly ulong Id;
        public readonly DateTime Time;
        public readonly ReadOnlyCollection<TransactionItem> Items;
        public readonly string Note;
        public readonly MoneyType MoneyType;
    }
}
