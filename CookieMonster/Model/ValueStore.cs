namespace CookieMonster.Model
{
    public class ValueStore {
        public ValueStore(ValueStoreType valueStoreType, ulong id, string name) {
            this.ValueStoreType = valueStoreType;
            this.Id = id;
            this.Name = name;
        }

        public ValueStoreType ValueStoreType { get; }
        public ulong Id { get; }
        public string Name { get; }
    }
}
