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

            var oreo = new Cookie(0, "Oreo", 3, 5.00M);
            var chip = new Cookie(0, "Chip", 7, 3.50M);
            var storedOreo = storage.PutCookie(oreo);
            Assert.AreEqual(oreo.BoxesPerCase, storedOreo.BoxesPerCase);
            Assert.AreEqual(oreo.PricePerBox, storedOreo.PricePerBox);
            Assert.AreEqual(oreo.Name, storedOreo.Name);
            Assert.AreEqual(1, storedOreo.Id);
            var storedChip = storage.PutCookie(chip);
            Assert.AreEqual(chip.BoxesPerCase, storedChip.BoxesPerCase);
            Assert.AreEqual(chip.PricePerBox, storedChip.PricePerBox);
            Assert.AreEqual(chip.Name, storedChip.Name);
            Assert.AreEqual(2, storedChip.Id);
            expectedCookies.Add(storedOreo);
            expectedCookies.Add(storedChip);
            CollectionAssert.AreEqual(expectedCookies.AsReadOnly(), storage.Cookies());

            expectedCookies.Remove(oreo);
            storage.DeleteCookie(oreo);
            CollectionAssert.AreEqual(expectedCookies.AsReadOnly(), storage.Cookies());
        }
    }
}
