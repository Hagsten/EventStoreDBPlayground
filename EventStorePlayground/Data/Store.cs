using EventStore.Client;
using EventStorePlayground.Domains;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventStorePlayground.Data
{
    public class Store
    {
        private readonly EventStoreClient _client;

        public Store()
        {
            _client = new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false&keepAliveInterval=-1&keepAliveTimeout=-1"));
        }

        private async Task<IReadOnlyCollection<ResolvedEvent>> GetSnapshotStream(string stream, string entityId, string model)
        {
            var streamName = model != null ? $"{stream}-{entityId}-{model}-snapshot" : $"{stream}-{entityId}-snapshot";

            var state = _client.ReadStreamAsync(
                Direction.Backwards,
                streamName,
                StreamPosition.End,
                1);

            var readState = await state.ReadState;

            if (readState == ReadState.StreamNotFound)
            {
                return new List<ResolvedEvent>();
            }

            return await state.ToListAsync();
        }

        public async Task<IReadOnlyCollection<ResolvedEvent>> GetEventStream(string stream, string entityId, string model = null)
        {
            var response = new List<ResolvedEvent>();

            var snapshots = await GetSnapshotStream(stream, entityId, model);

            var position = StreamPosition.Start;

            if (snapshots.Count > 0)
            {
                Console.WriteLine($"Using snapshot for model {model}");
                var meta = JsonConvert.DeserializeObject<SnapshotMetaData>(Encoding.UTF8.GetString(snapshots.Last().Event.Metadata.Span));
                position = new StreamPosition(meta.Position + 1);
                response.Add(snapshots.Last());
            }

            var state = _client.ReadStreamAsync(
                Direction.Forwards,
                $"{stream}-{entityId}",
                position);

            var readState = await state.ReadState;

            if (readState == ReadState.StreamNotFound)
            {
                return new List<ResolvedEvent>();
            }

            response.AddRange(await state.ToListAsync());

            return response;
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
                eventData, configureOperationOptions: (o) =>
                {
                    o.TimeoutAfter = TimeSpan.FromMinutes(5);
                });
        }

        public async Task Snapshot(string stream, string model, ISnapshotEvent snapshot)
        {
            var state = await _client.ReadStreamAsync(
                Direction.Backwards,
                $"{stream}",
                StreamPosition.End,
                1).ToListAsync();

            if (state.Count == 0)
            {
                Console.WriteLine("No need for a snapshot yet...");
                return;
            }

            var pos = state.Single().Event.EventNumber.ToUInt64();

            var eventData = new EventData(
                Uuid.NewUuid(),
                snapshot.GetType().Name,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(snapshot)),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new SnapshotMetaData(pos))));

            await _client.AppendToStreamAsync(
                $"{stream}-{model}-snapshot",
                StreamState.Any,
                new[] { eventData });
        }
    }

    public class SnapshotMetaData
    {
        public SnapshotMetaData(ulong position)
        {
            Position = position;
        }

        public ulong Position { get; }
    }
}
