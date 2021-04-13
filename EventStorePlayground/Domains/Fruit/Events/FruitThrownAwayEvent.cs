namespace EventStorePlayground.Domains.Fruit.Events
{
    public class FruitThrownAwayEvent : IDomainEvent
    {
        public FruitThrownAwayEvent(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
