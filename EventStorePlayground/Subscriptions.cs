using EventStore.Client;
using EventStorePlayground.Data;
using EventStorePlayground.Domain.Events;
using EventStorePlayground.Projections;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventStorePlayground
{
    public static class Subscriptions
    {
        private static Store _store;
        private static EventStoreClient _client;

        static Subscriptions()
        {
            _store = new Store();
            _client = new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false&keepAliveInterval=-1&keepAliveTimeout=-1"));
        }

        public static async Task WhenFruitGrabbed()
        {
            await _client.SubscribeToStreamAsync("$et-FruitGrabbedEvent", StreamPosition.End,
                async (subscription, evnt, cancellationToken) =>
                {
                    try
                    {
                        var e = JsonConvert.DeserializeObject<FruitGrabbedEvent>(Encoding.UTF8.GetString(evnt.Event.Data.Span));

                        var isEatable = await new FruitEatableProjection().Project(e.Id);

                        IDomainEvent newEvent = isEatable.Value ? new FruitEatenEvent(e.Id) : new FruitThrownAwayEvent(e.Id);

                        Console.WriteLine($"That fruit is {(isEatable.Value ? "fresh, lets eat it!" : "rotten, lets throw it away")}" + Environment.NewLine);

                        await _store.Add("fruit", e.Id, new[] { newEvent });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in subscr. " + e.Message);
                    }
                }, true);
        } 
        
        public static async Task ConsoleLogAllEvents()
        {
            await _client.SubscribeToAllAsync(Position.End, async (subscription, evnt, cancellationToken) =>
            {
                Console.WriteLine($"{Environment.NewLine}{evnt.Event.EventType} appended{Environment.NewLine}");

                await Task.CompletedTask;
            }, filterOptions: new SubscriptionFilterOptions(EventTypeFilter.RegularExpression("Event")));
        }
    }
}
