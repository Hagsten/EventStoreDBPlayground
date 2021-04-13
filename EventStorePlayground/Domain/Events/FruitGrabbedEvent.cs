namespace EventStorePlayground.Domain.Events
{
    public class FruitGrabbedEvent : ThingGrabbedEvent
    {
        public FruitGrabbedEvent(string id) : base(id)
        { }
    }
}
