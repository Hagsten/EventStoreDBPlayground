using EventStorePlayground.Data;
using EventStorePlayground.Domains.Fruit;
using EventStorePlayground.Projections;
using System.Threading.Tasks;

namespace EventStorePlayground.CommandHandlers
{
    public class EatFruitCommandHandler
    {
        private readonly Store _store;

        public EatFruitCommandHandler()
        {
            _store = new Store();
        }

        public async Task Handle(string fruitId)
        {
            var fruit = await new ProjectionFromFruit<FruitAggregate>(FruitAggregate.Replay).Project(fruitId);

            if(fruit == null)
            {
                throw new System.Exception($"{fruitId} was not found");
            }

            fruit.Eat();

            await _store.Add("fruit", fruitId, fruit.Events);
        }
    }
}
