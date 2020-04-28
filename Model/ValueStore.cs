class ValueStore {
    private ValueType _type;
    public readonly uint64 Id;
    public readonly string Name;

    public virtual ValueType GetType()
    {
        return _type;
    }
}
