using CookieMonster.Model;
using System.Collections.ObjectModel;

namespace CookieMonster.Storage {
    interface IStorage {
        ReadOnlyCollection<Cookie> Cookies();
        ReadOnlyCollection<ValueStore> ValueStores();
        ReadOnlyCollection<Transaction> Transactions ();

        void DeleteCookie(Cookie cookie);
        void DeleteValueStore(ValueStore valueStore);
        void DeleteTransaction(Transaction transaction);

        void PutCookie(Cookie cookie);
        void PutValueStore(ValueStore valueStore);
        void PutTransaction(Transaction transaction);
    }
}
