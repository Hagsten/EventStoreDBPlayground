namespace EventStorePlayground.Domain
{
    public class Key : IThing
    {
        public string Id { get; }
        public decimal Weight { get; }
        public string Owner { get; }

        public TypeOfThing TypeOfThing => TypeOfThing.Key;

        public Key(string id, decimal weight, string owner)
        {
            Id = id;
            Weight = weight;
            Owner = owner;
        }
    }
}
