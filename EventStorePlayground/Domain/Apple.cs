namespace EventStorePlayground.Domain
{
    public class Apple : IFruit
    {
        public string Id { get; }
        public decimal Weight { get; }
        public FruitCondition FruitCondition { get; }
        public TypeOfThing TypeOfThing => TypeOfThing.Fruit;

        public Apple(string id, decimal weight, FruitCondition fruitCondition)
        {
            Id = id;
            Weight = weight;
            FruitCondition = fruitCondition;
        }
    }
}
