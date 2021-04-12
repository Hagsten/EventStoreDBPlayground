namespace EventStorePlayground.Domain.Events
{
    public class AppleAddedEvent : ThingAddedEvent
    {
        public AppleAddedEvent(string id, decimal weight, FruitCondition fruitCondition) : base(id, TypeOfThing.Fruit, weight)
        {
            FruitCondition = fruitCondition;
        }

        public FruitCondition FruitCondition { get; }
    }

    public class FruitGrabbedEvent : ThingGrabbedEvent
    {
        public FruitGrabbedEvent(string id) : base(id)
        { }
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
