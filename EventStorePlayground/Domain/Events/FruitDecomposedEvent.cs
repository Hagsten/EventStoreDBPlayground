namespace EventStorePlayground.Domain.Events
{
    public class FruitDecomposedEvent : IDomainEvent
    {
        public string Id { get; }

        public FruitDecomposedEvent(string fruitId)
        {
            Id = fruitId;
        }
    }
}
