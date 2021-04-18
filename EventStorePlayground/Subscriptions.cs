using EventStore.Client;
using EventStorePlayground.CommandHandlers;
using EventStorePlayground.Data;
using EventStorePlayground.Domain.Events;
using EventStorePlayground.Domains.Basket.Events;
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

                        await new EatFruitCommandHandler().Handle(e.Id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in subscr. " + e.Message);
                    }
                }, true);
        }

        public static async Task WhenFruitAdded()
        {
            await _client.SubscribeToAllAsync(Position.End,
                async (subscription, evnt, cancellationToken) =>
                {
                    try
                    {
                        dynamic e = evnt.Event.EventType switch
                        {
                            "AppleAddedEvent" => JsonConvert.DeserializeObject<AppleAddedEvent>(Encoding.UTF8.GetString(evnt.Event.Data.Span)),
                            "PearAddedEvent" => JsonConvert.DeserializeObject<PearAddedEvent>(Encoding.UTF8.GetString(evnt.Event.Data.Span)),
                            _ => null
                        };
                        
                        Console.WriteLine($"{evnt.Event.EventNumber}");
                        
                        await new SummonFruitCommandHandler().Handle(e.Id, e.Weight, e.FruitCondition, TypeOfFruit.Apple);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in subscr. " + e.Message);
                    }
                }, resolveLinkTos: true, filterOptions: new SubscriptionFilterOptions(EventTypeFilter.RegularExpression("AppleAddedEvent|PearAddedEvent")));
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
