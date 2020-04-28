using System.Collections.Generic;
using System;

namespace CookieMonster.Model
{
    public class Transaction {
        public Transaction(ulong id,
                                 DateTime time,
                                 List<TransactionItem> items,
                                 string note,
                                 MoneyType moneyType,
                                 Transaction? linkedTransaction) {
            this.Id = id;
            this.Time = time;
            this.Items = items;
            this.Note = note;
            this.MoneyType = moneyType;
            this.LinkedTransaction = linkedTransaction;
        }

        ulong Id { get; }
        DateTime Time { get; }
        List<TransactionItem> Items { get; set; }
        string Note { get; set; }
        MoneyType MoneyType { get; }
        Transaction? LinkedTransaction { get; set; }
    }
}
