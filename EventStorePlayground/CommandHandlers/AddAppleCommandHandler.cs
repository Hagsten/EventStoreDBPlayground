using EventStorePlayground.Data;
using EventStorePlayground.Domains.Basket;
using EventStorePlayground.Projections;
using System.Threading.Tasks;

namespace EventStorePlayground.CommandHandlers
{
    public class AddAppleCommandHandler
    {
        private readonly Store _store;
        private readonly BasketProjection _basketProjection;

        public AddAppleCommandHandler()
        {
            _store = new Store();
            _basketProjection = new BasketProjection();
        }

        public async Task Handle(string basketId, string fruitId, decimal weight, FruitCondition condition)
        {
            var basket = await _basketProjection.Project(basketId);

            basket.AddFruit(new Apple(fruitId, weight, condition));

            await _store.Add("basket", basketId, basket.Events);
        }
    }
}
