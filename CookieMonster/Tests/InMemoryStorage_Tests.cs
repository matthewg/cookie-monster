using Microsoft.VisualStudio.TestTools.UnitTesting;
using CookieMonster.Model;
using CookieMonster.Storage;

namespace CookieMonster.Tests {
    [TestClass]
    public class InMemoryStorage_Tests {
        [TestMethod]
        public void TestBasic() {
            var storage = new InMemoryStorage();
            Assert.AreEqual(null, storage.Cookies());
            Assert.AreEqual(null, storage.ValueStores());
            Assert.AreEqual(null, storage.Transactions());
            Assert.AreEqual(null, storage.Currencies());
        }
    }
}
