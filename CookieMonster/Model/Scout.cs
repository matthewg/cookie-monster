namespace CookieMonster.Model {
    public class Scout {
        public Scout(ValueStore valueStore, string digitalCookieUrl) {
            this.ValueStore = valueStore;
            this.DigitalCookieUrl = digitalCookieUrl;
        }

        public readonly ValueStore ValueStore;
        public string DigitalCookieUrl { get; set; }
    }
}
