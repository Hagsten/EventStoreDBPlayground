namespace EventStorePlayground.Domain.Events
{
    public class KeyAddedEvent : ThingAddedEvent
    {
        public KeyAddedEvent(string id, decimal weight, string owner) : base(id, TypeOfThing.Key, weight)
        {
            Owner = owner;
        }

        public string Owner { get; }
    }
}
