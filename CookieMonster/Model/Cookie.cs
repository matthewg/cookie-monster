namespace CookieMonster.Model {
    public struct Cookie {
        public Cookie(ulong id, bool isCash, string name, int boxesPerCase, decimal pricePerBox) {
            this.Id = id;
            this.IsCash = isCash;
            this.Name = name;
            this.BoxesPerCase = boxesPerCase;
            this.PricePerBox = pricePerBox;
        }

        public ulong Id { get; }
        public bool IsCash { get; }
        public string Name { get; set; }
        public int BoxesPerCase { get; set; }
        public decimal PricePerBox { get; set; }
    }
}
