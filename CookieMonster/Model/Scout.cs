namespace CookieMonster.Model {
    public class Scout : ValueStore {
        public Scout(ulong id, string name, string digitalCookieUrl)
          : base(ValueStoreType.Scout, id, name){
            this.DigitalCookieUrl = digitalCookieUrl;
        }

        public string DigitalCookieUrl { get; set; }
    }
}
