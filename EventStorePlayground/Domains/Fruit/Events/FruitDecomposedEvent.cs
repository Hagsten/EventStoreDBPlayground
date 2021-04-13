namespace EventStorePlayground.Domains.Fruit.Events
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
