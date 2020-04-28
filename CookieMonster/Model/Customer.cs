namespace CookieMonster.Model {
    public class Customer : ValueStore {
        public Customer(ulong id, string name, string address, string phoneNumber, string email, string notes)
          : base(ValueStoreType.Customer, id, name) {
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.Notes = notes;
        }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}
