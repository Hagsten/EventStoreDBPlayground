using EventStorePlayground.Domains.Basket;
using EventStorePlayground.Domains.Basket.Events;
using EventStorePlayground.Domains.Fruit.Events;
using System.Collections.Generic;

namespace EventStorePlayground.Domains.Fruit
{
    public class FruitAggregate
    {
        private readonly List<IDomainEvent> _events;

        public decimal Weight { get; private set; }
        public string Id { get; private set; }
        public FruitCondition FruitCondition { get; private set; }
        public TypeOfFruit TypeOfFruit { get; private set; }
        public bool IsEaten { get; private set; }
        public bool InTrash { get; private set; }

        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        public FruitAggregate()
        {
            _events = new List<IDomainEvent>();
        }

        public static FruitAggregate CreateNew(string fruitId, decimal weight, FruitCondition condition, TypeOfFruit type)
        {
            var agg = new FruitAggregate();

            agg.Summon(fruitId, weight, condition, type);

            return agg;
        }

        public static FruitAggregate Replay(ICollection<IDomainEvent> events)
        {
            var agg = new FruitAggregate();

            foreach (var e in events)
            {
                agg.Apply((dynamic)e);
            }

            return agg;
        }

        private void Apply(FruitCreatedEvent e)
        {
            Weight = e.Weight;
            Id = e.Id;
            FruitCondition = e.Condition;
            TypeOfFruit = e.TypeOfFruit;
        }

        private void Apply(FruitDecomposedEvent e)
        {
            if (e.Id != Id)
            {
                throw new System.Exception("Does not belong to this entity");
            }

            FruitCondition = FruitCondition.Decomposed;
        }

        private void Apply(FruitEatenEvent e)
        {
            if (e.Id != Id)
            {
                throw new System.Exception("Does not belong to this entity");
            }

            IsEaten = true;
        }

        private void Apply(FruitThrownAwayEvent e)
        {
            if (e.Id != Id)
            {
                throw new System.Exception("Does not belong to this entity");
            }

            InTrash = true;
        }

        private void Summon(string fruitId, decimal weight, FruitCondition condition, TypeOfFruit type)
        {
            var ev = new FruitCreatedEvent(fruitId, weight, condition, type);

            _events.Add(ev);

            Apply((dynamic)ev);
        }

        public void Eat()
        {
            var isEatable = FruitCondition == FruitCondition.Fresh || FruitCondition == FruitCondition.Mature;

            IDomainEvent ev = isEatable ? new FruitEatenEvent(Id) : new FruitThrownAwayEvent(Id);

            _events.Add(ev);

            Apply((dynamic)ev);           
        }
    }
}
