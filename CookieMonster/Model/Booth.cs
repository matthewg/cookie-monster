using System.Collections.Generic;
using System;

namespace CookieMonster.Model {
    public class Booth : ValueStore {
        public Booth(ulong id, string name, string location, string notes, DateTime shiftTime, List<Scout> scouts)
          : base(ValueStoreType.Booth, id, name) {
            this.Location = location;
            this.Notes = notes;
            this.ShiftTime = shiftTime;
            this.Scouts = scouts;
        }

        public string Location { get; set; }
        public string Notes { get; set; }
        public DateTime ShiftTime { get; set; }
        public List<Scout> Scouts { get; set; }
    }
}
