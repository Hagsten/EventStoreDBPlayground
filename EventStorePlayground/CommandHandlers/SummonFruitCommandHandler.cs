using EventStorePlayground.Data;
using EventStorePlayground.Domains.Basket;
using EventStorePlayground.Domains.Fruit;
using EventStorePlayground.Projections;
using System.Threading.Tasks;

namespace EventStorePlayground.CommandHandlers
{
    public class SummonFruitCommandHandler
    {
        private readonly Store _store;

        public SummonFruitCommandHandler()
        {
            _store = new Store();
        }

        public async Task Handle(string fruitId, decimal weight, FruitCondition condition, Domains.Basket.Events.TypeOfFruit typeOfFruit)
        {
            var fruit = await new ProjectionFromFruit<FruitAggregate>(FruitAggregate.Replay).Project(fruitId);

            if(fruit != null)
            {
                throw new System.Exception("Can not summon a fruit that exists");
            }

            var agg = FruitAggregate.CreateNew(fruitId, weight, condition, typeOfFruit);

            await _store.Add("fruit", fruitId, agg.Events);
        }
    }
}
