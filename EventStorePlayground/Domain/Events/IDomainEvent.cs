namespace EventStorePlayground.Domain.Events
{
    public interface IDomainEvent
    {
        string Id { get; }
    }
}
