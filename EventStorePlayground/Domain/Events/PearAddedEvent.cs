namespace EventStorePlayground.Domain.Events
{
    public class PearAddedEvent : IDomainEvent
    {
        public PearAddedEvent(string id, decimal weight, FruitCondition fruitCondition)
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
