using EventStore.Client;
using EventStorePlayground.Domains;
using EventStorePlayground.Domains.Basket.Events;
using EventStorePlayground.Domains.Basket.Events.Snapshot;
using EventStorePlayground.Domains.Fruit.Events;
using Newtonsoft.Json;
using System;
using System.Text;

namespace EventStorePlayground.Projections
{
    public static class EventDeserializer
    {
        public static IDomainEvent Deserialize(ResolvedEvent e)
        {
            return e.Event.EventType switch
            {
                nameof(AllThingsEverInBasketSbapshotEvent) => JsonConvert.DeserializeObject<AllThingsEverInBasketSbapshotEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(AppleAddedEvent) => JsonConvert.DeserializeObject<AppleAddedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(PearAddedEvent) => JsonConvert.DeserializeObject<PearAddedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(ThingGrabbedEvent) => JsonConvert.DeserializeObject<ThingGrabbedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(FruitGrabbedEvent) => JsonConvert.DeserializeObject<FruitGrabbedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(KeyAddedEvent) => JsonConvert.DeserializeObject<KeyAddedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(UnknownFruitAddedEvent) => JsonConvert.DeserializeObject<UnknownFruitAddedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(FruitEatenEvent) => JsonConvert.DeserializeObject<FruitEatenEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(FruitDecomposedEvent) => JsonConvert.DeserializeObject<FruitDecomposedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(FruitThrownAwayEvent) => JsonConvert.DeserializeObject<FruitThrownAwayEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                nameof(FruitCreatedEvent) => JsonConvert.DeserializeObject<FruitCreatedEvent>(Encoding.UTF8.GetString(e.Event.Data.Span)),
                _ => throw new NotImplementedException()
            };
        }
    }
}
