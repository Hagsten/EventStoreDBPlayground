namespace EventStorePlayground.Domain.Events
{
    public class AppleAddedEvent : ThingAddedEvent
    {
        public AppleAddedEvent(string id, decimal weight, FruitCondition fruitCondition) : base(id, TypeOfThing.Fruit, weight)
        {
            FruitCondition = fruitCondition;
        }

        public FruitCondition FruitCondition { get; }
    }
}
