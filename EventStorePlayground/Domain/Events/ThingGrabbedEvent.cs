namespace EventStorePlayground.Domain.Events
{
    public class ThingGrabbedEvent : IDomainEvent
    {
        public ThingGrabbedEvent(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
