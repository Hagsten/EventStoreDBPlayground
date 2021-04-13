using EventStorePlayground.Domain.Events;

namespace EventStorePlayground.Domains.Basket.Events
{
    public class PearAddedEvent : ThingAddedEvent
    {
        public PearAddedEvent(string id, decimal weight, FruitCondition fruitCondition)
             : base(id, TypeOfThing.Fruit, weight)
        {
            FruitCondition = fruitCondition;
        }

        public FruitCondition FruitCondition { get; }
    }
}
