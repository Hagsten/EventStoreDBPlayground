namespace EventStorePlayground.Domain.Events
{
    public class FruitEatenEvent : IDomainEvent
    {
        public FruitEatenEvent(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
