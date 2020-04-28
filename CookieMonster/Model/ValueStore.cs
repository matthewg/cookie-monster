using System;

namespace CookieMonster.Model
{
    public struct ValueStore {
        public ValueStore(ValueStoreType valueStoreType, ulong id, string name) {
            this.ValueStoreType = valueStoreType;
            this.Id = id;
            this.Name = name;
        }

        public readonly ValueStoreType ValueStoreType;
        public readonly ulong Id;
        public readonly string Name;
    }
}
