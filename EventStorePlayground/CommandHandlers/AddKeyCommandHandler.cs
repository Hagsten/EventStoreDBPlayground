using EventStorePlayground.Data;
using EventStorePlayground.Domain;
using EventStorePlayground.Domains.Basket;
using EventStorePlayground.Projections;
using System.Threading.Tasks;

namespace EventStorePlayground.CommandHandlers
{
    public class AddKeyCommandHandler
    {
        private readonly Store _store;
        private readonly BasketProjection _basketProjection;

        public AddKeyCommandHandler()
        {
            _store = new Store();
            _basketProjection = new BasketProjection();
        }

        public async Task Handle(string basketId, string id, decimal weight, string owner)
        {
            var basket = await _basketProjection.Project(basketId);

            basket.AddKey(new Key(id, weight, owner));

            await _store.Add("basket", basketId, basket.Events);
        }
    }
}
