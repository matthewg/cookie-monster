class Customer : ValueStore {
    public readonly uint64 Id;
    public string Address;
    public string PhoneNumber;
    public string Email;
    public string Notes;

    public override ValueType GetType()
    {
        return ValueType.Customer;
    }
}
