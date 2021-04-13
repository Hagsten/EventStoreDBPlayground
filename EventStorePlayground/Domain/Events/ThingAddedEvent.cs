namespace EventStorePlayground.Domain.Events
{
    public abstract class ThingAddedEvent : IDomainEvent
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
