using CookieMonster.Model;
using System.Collections.ObjectModel;

namespace CookieMonster.Storage {
    public interface IStorage {
        ReadOnlyCollection<Booth> Booths();
        ReadOnlyCollection<Cookie> Cookies();
        ReadOnlyCollection<Customer> Customers();
        ReadOnlyCollection<Scout> Scouts();
        ReadOnlyCollection<Transaction> Transactions();
        ReadOnlyCollection<TransactionItem> TransactionItems(ValueStore valueStore);
        ReadOnlyCollection<ValueStore> ValueStores();

        void DeleteBooth(Booth booth);
        void DeleteCookie(Cookie cookie);
        void DeleteCustomer(Customer customer);
        void DeleteScout(Scout scout);
        void DeleteTransaction(Transaction transaction);
        void DeleteValueStore(ValueStore valueStore);

        Booth PutBooth(Booth booth);
        Cookie PutCookie(Cookie cookie);
        Customer PutCustomer(Customer customer);
        Scout PutScout(Scout scout);
        Transaction PutTransaction(Transaction transaction);
        ValueStore PutValueStore(ValueStore valueStore);
    }
}
