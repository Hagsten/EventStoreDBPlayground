using EventStore.Client;
using EventStorePlayground.Domain;
using EventStorePlayground.Domain.Events;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
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

            var events = eventStream.Select(Deserialize).ToArray();

            return Basket.Replay(events);
        }

        private static IDomainEvent Deserialize(ResolvedEvent e)
        {
            return e.Event.EventType switch
            {
                nameof(AppleAddedEvent) => JsonConvert.DeserializeObject<AppleAddedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(PearAddedEvent) => JsonConvert.DeserializeObject<PearAddedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(ThingGrabbedEvent) => JsonConvert.DeserializeObject<ThingGrabbedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(UnknownFruitAddedEvent) => JsonConvert.DeserializeObject<UnknownFruitAddedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                _ => throw new NotImplementedException()
            };
        }
    }
}
