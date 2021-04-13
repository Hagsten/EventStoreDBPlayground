namespace EventStorePlayground.Domains.Basket.Events
{
    public class FruitGrabbedEvent : ThingGrabbedEvent
    {
        public FruitGrabbedEvent(string id) : base(id)
        { }
    }
}
