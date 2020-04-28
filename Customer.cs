class Customer : ValueStore {
    public string Address;
    public string PhoneNumber;
    public string Email;
    public string Notes;

    public override ValueType GetType()
    {
        return ValueType.Customer;
    }
}
