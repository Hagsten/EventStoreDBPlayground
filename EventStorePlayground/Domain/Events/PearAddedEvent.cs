namespace EventStorePlayground.Domain.Events
{
    public class PearAddedEvent : ThingAddedEvent
    {
        public PearAddedEvent(string id, decimal weight, FruitCondition fruitCondition)
             : base(id, TypeOfThing.Fruit, weight)
        {
            FruitCondition = fruitCondition;
        }

        public FruitCondition FruitCondition { get; }
    }

    public class ThingAddedEvent : IDomainEvent
    {
        public TypeOfThing TypeOfThing { get; }
        public string Id { get; }
        public decimal Weight { get; }

        public ThingAddedEvent(string id, TypeOfThing typeOfThing, decimal weight)
        {
            Id = id;
            TypeOfThing = typeOfThing;
            Weight = weight;
        }
    }
}
