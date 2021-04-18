namespace EventStorePlayground.Domains.Basket
{
    public interface IThing
    {
        decimal Weight { get; }
        TypeOfThing TypeOfThing { get; }
        string Id { get; }
    }
}
