using CookieMonster.Model;
using System.Collections.ObjectModel;

namespace CookieMonster.Storage {
    interface IStorage {
        ReadOnlyCollection<Booth> Booths();
        ReadOnlyCollection<Cookie> Cookies();
        ReadOnlyCollection<Customer> Customers();
        ReadOnlyCollection<Scout> Scouts();
        ReadOnlyCollection<Transaction> Transactions();
        ReadOnlyCollection<ValueStore> ValueStores();

        void DeleteBooth(Booth booth);
        void DeleteCookie(Cookie cookie);
        void DeleteCustomer(Customer customer);
        void DeleteScout(Scout scout);
        void DeleteTransaction(Transaction transaction);
        void DeleteValueStore(ValueStore valueStore);

        void PutBooth(Booth booth);
        void PutCookie(Cookie cookie);
        void PutCustomer(Customer customer);
        void PutScout(Scout scout);
        void PutTransaction(Transaction transaction);
        void PutValueStore(ValueStore valueStore);
    }
}
