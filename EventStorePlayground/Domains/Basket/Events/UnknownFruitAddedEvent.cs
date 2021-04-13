using EventStorePlayground.Domain.Events;

namespace EventStorePlayground.Domains.Basket.Events
{
    public class UnknownFruitAddedEvent : ThingAddedEvent
    {
        public UnknownFruitAddedEvent(string id, decimal weight, FruitCondition fruitCondition)
            : base(id, TypeOfThing.Fruit, weight)
        {
            FruitCondition = fruitCondition;
        }

        public FruitCondition FruitCondition { get; }
    }
}
