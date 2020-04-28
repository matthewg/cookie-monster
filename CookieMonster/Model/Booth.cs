using System.Collections.Generic;
using System;

namespace CookieMonster.Model {
    public class Booth {
        public Booth(ValueStore valueStore, string location, string notes, DateTime shiftTime, List<Scout> scouts) {
            this.ValueStore = valueStore;
            this.Location = location;
            this.Notes = notes;
            this.ShiftTime = shiftTime;
            this.Scouts = new List<Scout>(scouts);
        }

        public readonly ValueStore ValueStore;
        public string Location { get; set; }
        public string Notes { get; set; }
        public DateTime ShiftTime { get; set; }
        public List<Scout> Scouts { get; set; }
    }
}
