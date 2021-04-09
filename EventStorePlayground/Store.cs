using EventStore.Client;
using EventStorePlayground.Domain.Events;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStorePlayground
{
    public class Store
    {
        private readonly EventStoreClient _client;

        public Store()
        {
            _client = new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false&keepAliveInterval=-1&keepAliveTimeout=-1"));
        }

        public async Task<IReadOnlyCollection<ResolvedEvent>> GetEventStream(string stream, string entityId)
        {
            var state = _client.ReadStreamAsync(
                Direction.Forwards,
                $"{stream}-{entityId}",
                StreamPosition.Start);

            var readState = await state.ReadState;

            if (readState == ReadState.StreamNotFound)
            {
                return new List<ResolvedEvent>();
            }

            return await state.ToListAsync();
        }

        public async Task Add(string stream, string entityId, IReadOnlyCollection<IDomainEvent> events)
        {
            var eventData = events.Select(e => new EventData(
                Uuid.NewUuid(),
                e.GetType().Name,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)))).ToArray();

            await _client.AppendToStreamAsync(
                $"{stream}-{entityId}",
                StreamState.Any,
                eventData);
        }
    }
}
