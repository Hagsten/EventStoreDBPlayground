using EventStorePlayground.Data;
using EventStorePlayground.Domain;
using EventStorePlayground.Domains.Basket;
using EventStorePlayground.Projections;
using System.Threading.Tasks;

namespace EventStorePlayground.CommandHandlers
{
    public class AddPearCommandHandler
    {
        private readonly Store _store;
        private readonly BasketProjection _basketProjection;

        public AddPearCommandHandler()
        {
            _store = new Store();
            _basketProjection = new BasketProjection();
        }

        public async Task Handle(string basketId, string fruitId, decimal weight, FruitCondition condition)
        {
            var basket = await _basketProjection.Project(basketId);

            basket.AddFruit(new Pear(fruitId, weight, condition));

            await _store.Add("basket", basketId, basket.Events);
        }
    }
}
