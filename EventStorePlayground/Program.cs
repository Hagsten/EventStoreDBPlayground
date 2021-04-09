using EventStore.Client;
using EventStorePlayground.CommandHandlers;
using EventStorePlayground.Domain.Events;
using EventStorePlayground.Projections;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStorePlayground
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello EventStoreDB!");

            SubscribeToEvents().Wait();

            RenderMenu().Wait();
        }

        private static async Task SubscribeToEvents()
        {
            var store = new Store();
            var client = new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false&keepAliveInterval=-1&keepAliveTimeout=-1"));

            await client.SubscribeToStreamAsync("$et-ThingGrabbedEvent", StreamPosition.End,
                async (subscription, evnt, cancellationToken) =>
                {
                    Console.WriteLine($"Received event {evnt.OriginalEventNumber}@{evnt.OriginalStreamId}");

                    try
                    {
                        var e = JsonConvert.DeserializeObject<ThingGrabbedEvent>(Encoding.UTF8.GetString(evnt.Event.Data.Span));

                        await store.Add("fruit", e.Id, new[] { new FruitEatenEvent(e.Id) });
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error in subscr.");
                    }
                });
        }

        public static async Task RenderMenu()
        {
            Console.WriteLine("Good day! This is your ultimate basket app");

            await FetchBasket();

            var cmd = "";

            Console.WriteLine("{Thing}.{Action}.{Property} ===> i.e Apple.A.F ==> Adds a Fresh Apple");
            Console.WriteLine("A:Key ===> Adds a key");

            while ((cmd = Console.ReadLine()) != "exit")
            {
                if (cmd == "Apple.A.F")
                {
                    Console.Write("Adding a nice fresh apple. Give it a unique name: ");

                    var id = Console.ReadLine();

                    await new AddAppleCommandHandler().Handle("kitchen-basket", id, 0.2M, FruitCondition.Fresh);
                }

                if (cmd == "Pear.A.F")
                {
                    Console.WriteLine("Adding a nice fresh large pear. Give it a unique name: ");

                    var id = Console.ReadLine();

                    await new AddPearCommandHandler().Handle("kitchen-basket", id, 0.34M, FruitCondition.Fresh);
                }

                if (cmd == "Pear.E" || cmd == "Apple.E")
                {
                    Console.WriteLine("Which one do you want to eat?");

                    await new EatFruitCommandHandler().Handle("kitchen-basket", Console.ReadLine());

                    Console.WriteLine("I bet that was yummy!");
                }

                await FetchBasket();
            }
        }

        private static async Task FetchBasket()
        {
            var basketProjection = new BasketProjection();

            var b = await basketProjection.Project("kitchen-basket");

            Console.WriteLine($"There are currently {b.Things.Count} things in your basket. Total weight {b.TotalWeight} kg.");

            var apples = b.Things.Where(x => GetTypeOfThing(x) == "Apple").ToArray();
            var pears = b.Things.Where(x => GetTypeOfThing(x) == "Pear").ToArray();

            Console.WriteLine($"Apples: {apples.Length} with weight of {apples.Sum(x => x.Weight)}kg");
            Console.WriteLine($"Pears: {pears.Length} with weight of {pears.Sum(x => x.Weight)}kg");

            Console.WriteLine("What do you want to do?");
        }

        private static string GetTypeOfThing(IThing thing)
        {
            return thing switch
            {
                Apple => "Apple",
                Pear => "Pear",
                _ => "Unknown",
            };
        }

    }

    public interface IThing
    {
        decimal Weight { get; }
        string Id { get; }
    }

    public interface IFruit : IThing
    {
        FruitCondition FruitCondition { get; }
    }

    public enum FruitCondition
    {
        Fresh = 0,
        Mature,
        Rotten,
        Decomposed
    }

    public class Apple : IFruit
    {
        public string Id { get; }
        public decimal Weight { get; }
        public FruitCondition FruitCondition { get; }

        public Apple(string id, decimal weight, FruitCondition fruitCondition)
        {
            Id = id;
            Weight = weight;
            FruitCondition = fruitCondition;
        }
    }

    public class Pear : IFruit
    {
        public string Id { get; }
        public decimal Weight { get; }
        public FruitCondition FruitCondition { get; }

        public Pear(string id, decimal weight, FruitCondition fruitCondition)
        {
            Id = id;
            Weight = weight;
            FruitCondition = fruitCondition;
        }
    }

    public class SomeFruit : IFruit
    {
        public string Id { get; }
        public decimal Weight { get; }
        public FruitCondition FruitCondition { get; }

        public SomeFruit(string id, decimal weight, FruitCondition fruitCondition)
        {
            Id = id;
            Weight = weight;
            FruitCondition = fruitCondition;
        }
    }

    public class Key : IThing
    {
        public string Id { get; }
        public decimal Weight { get; }

        public Key(string id, decimal weight)
        {
            Id = id;
            Weight = weight;
        }

    }
}
