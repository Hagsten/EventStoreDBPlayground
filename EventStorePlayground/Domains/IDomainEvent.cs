namespace EventStorePlayground.Domains
{
    public interface IDomainEvent
    {
        string Id { get; }
    }

    public interface ISnapshotEvent : IDomainEvent
    {}
}
