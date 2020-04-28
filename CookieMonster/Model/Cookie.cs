namespace CookieMonster.Model {
    public struct Cookie {
        public Cookie(ulong id, string name, int boxesPerCase, decimal pricePerBox) {
            this.Id = id;
            this.Name = name;
            this.BoxesPerCase = boxesPerCase;
            this.PricePerBox = pricePerBox;
        }

        public readonly ulong Id;
        public readonly string Name;
        public readonly int BoxesPerCase;
        public readonly decimal PricePerBox;

        public static Cookie CASH {
            get => new Cookie(0, "USD", 1, 1.0M);
        }
    }
}
