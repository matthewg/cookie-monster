class ValueStore {
    private ValueType _type;
    public readonly string Name;

    public virtual ValueType GetType()
    {
        return _type;
    }
}
