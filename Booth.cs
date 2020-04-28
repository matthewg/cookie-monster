class Booth : ValueStore {
    public string Location;
    public string Notes;
    public DateTime ShiftTime;
    public List<Scout> Scouts;
    public override ValueType GetType()
    {
        return ValueType.Booth;
    }
}
