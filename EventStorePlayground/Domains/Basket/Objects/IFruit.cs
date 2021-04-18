namespace EventStorePlayground.Domains.Basket
{
    public interface IFruit : IThing
    {
        FruitCondition FruitCondition { get; }
    }
}
