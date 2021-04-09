namespace EventStorePlayground.Domain.Events
{
    public class UnknownFruitAddedEvent : IDomainEvent
    {
        public UnknownFruitAddedEvent(string id, decimal weight, FruitCondition fruitCondition)
        {
            Id = id;
            Weight = weight;
            FruitCondition = fruitCondition;
        }

        public string Id { get; }
        public decimal Weight { get; }
        public FruitCondition FruitCondition { get; }
    }
}
