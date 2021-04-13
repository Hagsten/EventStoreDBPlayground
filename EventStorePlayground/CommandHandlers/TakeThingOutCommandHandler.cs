using EventStorePlayground.Data;
using EventStorePlayground.Projections;
using System.Threading.Tasks;

namespace EventStorePlayground.CommandHandlers
{
    public class TakeThingOutCommandHandler
    {
        private readonly Store _store;
        private readonly BasketProjection _basketProjection;

        public TakeThingOutCommandHandler()
        {
            _store = new Store();
            _basketProjection = new BasketProjection();
        }

        public async Task Handle(string basketId, string fruitId)
        {
            var basket = await _basketProjection.Project(basketId);

            basket.GrabAThing(fruitId);

            await _store.Add("basket", basketId, basket.Events);           
        }
    }
}
