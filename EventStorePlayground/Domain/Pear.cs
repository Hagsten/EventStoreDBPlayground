namespace EventStorePlayground.Domain
{
    public class Pear : IFruit
    {
        public string Id { get; }
        public decimal Weight { get; }
        public FruitCondition FruitCondition { get; }
        public TypeOfThing TypeOfThing => TypeOfThing.Fruit;

        public Pear(string id, decimal weight, FruitCondition fruitCondition)
        {
            Id = id;
            Weight = weight;
            FruitCondition = fruitCondition;
        }
    }
}
