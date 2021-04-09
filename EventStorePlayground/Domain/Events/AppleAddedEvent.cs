namespace EventStorePlayground.Domain.Events
{
    public class AppleAddedEvent : IDomainEvent
    {
        public AppleAddedEvent(string id, decimal weight, FruitCondition fruitCondition)
        {
            Id = id;
            Weight = weight;
            FruitCondition = fruitCondition;
        }

        public string Id { get; }
        public decimal Weight { get; }
        public FruitCondition FruitCondition { get; }
    }

    public class ThingGrabbedEvent : IDomainEvent
    {
        public ThingGrabbedEvent(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
