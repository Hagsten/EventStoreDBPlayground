using EventStorePlayground.Data;
using EventStorePlayground.Domain.Aggregates;
using System.Linq;
using System.Threading.Tasks;

namespace EventStorePlayground.Projections
{
    public class BasketProjection
    {
        private readonly Store _store;

        public BasketProjection()
        {
            _store = new Store();
        }

        public async Task<Basket> Project(string baskedId)
        {
            var eventStream = await _store.GetEventStream("basket", baskedId);

            var events = eventStream.Select(EventDeserializer.Deserialize).ToArray();

            return Basket.Replay(events);
        }       
    }
}
