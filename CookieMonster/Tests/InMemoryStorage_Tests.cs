using Microsoft.VisualStudio.TestTools.UnitTesting;
using CookieMonster.Model;
using CookieMonster.Storage;
using System.Collections.Generic;

namespace CookieMonster.Tests {
    [TestClass]
    public class InMemoryStorage_Tests {
        [TestMethod]
        public void TestCookies() {
            var expectedCookies = new List<Cookie>();

            // Store should be initialized empty.
            var storage = new InMemoryStorage();
            CollectionAssert.AreEqual(expectedCookies.AsReadOnly(), storage.Cookies());

            var oreo = new Cookie(1, "Oreo", 3, 5.00M);
            var chip = new Cookie(2, "Chip", 7, 3.50M);
            expectedCookies.Add(oreo);
            expectedCookies.Add(chip);
            storage.PutCookie(oreo);
            storage.PutCookie(chip);
            CollectionAssert.AreEqual(expectedCookies.AsReadOnly(), storage.Cookies());

            expectedCookies.Remove(oreo);
            storage.DeleteCookie(oreo);
            CollectionAssert.AreEqual(expectedCookies.AsReadOnly(), storage.Cookies());
        }

        [TestMethod]
        public void TestValueStores() {
            var expectedCookies = new List<Cookie>();

            // Store should be initialized empty.
            var storage = new InMemoryStorage();
            CollectionAssert.AreEqual(expectedCookies.AsReadOnly(), storage.Cookies());

            var oreo = new Cookie(1, "Oreo", 3, 5.00M);
            var chip = new Cookie(2, "Chip", 7, 3.50M);
            expectedCookies.Add(oreo);
            expectedCookies.Add(chip);
            storage.PutCookie(oreo);
            storage.PutCookie(chip);
            CollectionAssert.AreEqual(expectedCookies.AsReadOnly(), storage.Cookies());

            // Mutating this list shouldn't mutate the data store.
            expectedCookies.Remove(oreo);
            CollectionAssert.AreNotEqual(expectedCookies.AsReadOnly(), storage.Cookies());
            // Now make the corresponding update to the data store, they should be equal again.
            storage.DeleteCookie(oreo);
            CollectionAssert.AreEqual(expectedCookies.AsReadOnly(), storage.Cookies());
        }
    }
}
