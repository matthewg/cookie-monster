using System;

namespace CookieMonster.Model {
    public class Customer {
        public Customer(ValueStore valueStore, string address, string phoneNumber, string email, string notes) {
            this.ValueStore = valueStore;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.Notes = notes;
        }

        public readonly ValueStore ValueStore;
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}
