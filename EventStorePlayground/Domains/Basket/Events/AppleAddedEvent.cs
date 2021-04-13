using EventStorePlayground.Domain.Events;

namespace EventStorePlayground.Domains.Basket.Events
{
    public class AppleAddedEvent : ThingAddedEvent
    {
        public AppleAddedEvent(string id, decimal weight, FruitCondition fruitCondition) : base(id, TypeOfThing.Fruit, weight)
        {
            FruitCondition = fruitCondition;
        }

        public FruitCondition FruitCondition { get; }
    }

    public enum TypeOfFruit
    {
        Apple = 0,
        Pear
    };
}
