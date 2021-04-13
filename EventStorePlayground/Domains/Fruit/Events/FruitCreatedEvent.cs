using EventStorePlayground.Domains.Basket;
using EventStorePlayground.Domains.Basket.Events;

namespace EventStorePlayground.Domains.Fruit.Events
{
    public class FruitCreatedEvent : IDomainEvent
    {
        public FruitCreatedEvent(string id, decimal weight, FruitCondition condition, TypeOfFruit typeOfFruit)
        {
            Id = id;
            Weight = weight;
            Condition = condition;
            TypeOfFruit = typeOfFruit;
        }

        public string Id { get; }
        public decimal Weight { get; }
        public FruitCondition Condition { get; }
        public TypeOfFruit TypeOfFruit { get; }
    }
}
