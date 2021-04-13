namespace EventStorePlayground.Domain
{
    public interface IFruit : IThing
    {
        FruitCondition FruitCondition { get; }
    }
}
