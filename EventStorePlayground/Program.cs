using EventStorePlayground.CommandHandlers;
using EventStorePlayground.Domain;
using EventStorePlayground.Projections;
using EventStorePlayground.ReadModels;
using System;
using System.Threading.Tasks;

namespace EventStorePlayground
{
    class Program
    {
        private const string BasketId = "kitchen-basket";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello EventStoreDB!" + Environment.NewLine);

            Subscriptions.ConsoleLogAllEvents().Wait();
            Subscriptions.WhenFruitGrabbed().Wait();

            RenderMenu().Wait();
        }

        public static async Task RenderMenu()
        {
            var p = new ProjectionFromBasket<AllThingsEverPutIntoBasket>(AllThingsEverPutIntoBasket.Replay);

            var allStuff = await p.Project(BasketId);

            Console.WriteLine("Good day! This is your ultimate basket app" + Environment.NewLine);
            
            await FetchBasket();
            
            Console.Write("What thing are you interested in? (Apple|Pear|key): ");
            string cmd;

            while ((cmd = Console.ReadLine().ToLower()) != "exit")
            {
                try
                {
                    if (cmd == "apple")
                    {
                        Console.Write("Apples! Do you want to add (a) or take (t) one out?: ");

                        var grab = Console.ReadLine().ToLower() == "t";

                        if (grab)
                        {
                            var apples = await new ProjectionFromBasket<ApplesInBasket>(ApplesInBasket.Replay).Project(BasketId);

                            Console.WriteLine("Choose from these apples:");

                            Console.Write(string.Join($"{Environment.NewLine}", apples.AppleIds));
                            Console.Write(Environment.NewLine + Environment.NewLine + "Which one?: ");

                            await new TakeThingOutCommandHandler().Handle(BasketId, Console.ReadLine());
                        }
                        else
                        {
                            Console.WriteLine("Adding a nice fresh apple.");
                            Console.Write("Give it a unique name: ");

                            var id = Console.ReadLine();

                            Console.Write("How much does it weight (kg)?: ");

                            var weight = decimal.Parse(Console.ReadLine());

                            await new AddAppleCommandHandler().Handle(BasketId, id, weight, FruitCondition.Fresh);
                        }
                    }

                    else if (cmd == "pear")
                    {
                        Console.Write("Pears! Do you want to add (a) or take (t) one out?: ");

                        var grab = Console.ReadLine().ToLower() == "t";

                        if (grab)
                        {
                            var pears = await new ProjectionFromBasket<PearsInBasket>(PearsInBasket.Replay).Project(BasketId);

                            Console.WriteLine("Choose from these pears:");

                            Console.Write(string.Join($"{Environment.NewLine}", pears.PearIds));
                            Console.Write(Environment.NewLine + Environment.NewLine + "Which one?: ");

                            await new TakeThingOutCommandHandler().Handle(BasketId, Console.ReadLine());
                        }
                        else
                        {
                            Console.WriteLine("Adding a nice fresh pear.");
                            Console.Write("Give it a unique name: ");

                            var id = Console.ReadLine();

                            Console.Write("How much does it weight (kg)?: ");

                            var weight = decimal.Parse(Console.ReadLine());

                            await new AddPearCommandHandler().Handle(BasketId, id, weight, FruitCondition.Fresh);
                        }
                    }

                    else if (cmd == "key")
                    {
                        Console.Write("Keys! In the fruit basket?! Do you want to add (a) or take (t) one out?: ");

                        var grab = Console.ReadLine().ToLower() == "t";

                        if (grab)
                        {
                            var keys = await new ProjectionFromBasket<KeysInBasket>(KeysInBasket.Replay).Project(BasketId);

                            Console.WriteLine("Choose from these keys:");

                            Console.Write(string.Join($"{Environment.NewLine}", keys.KeyIds));
                            Console.Write(Environment.NewLine + Environment.NewLine + "Which one?: ");

                            await new TakeThingOutCommandHandler().Handle(BasketId, Console.ReadLine());
                        }
                        else
                        {
                            Console.WriteLine("Adding a key.");
                            Console.Write("Give it a unique name: ");

                            var id = Console.ReadLine();

                            Console.Write("How much does it weight (kg)?: ");

                            var weight = decimal.Parse(Console.ReadLine());

                            Console.Write("Who is the owner of this key?: ");

                            var owner = Console.ReadLine();

                            await new AddKeyCommandHandler().Handle(BasketId, id, weight, owner);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No such command");
                    }

                    await Task.Delay(500);                                        
                }
                catch (Exception e)
                {
                    Console.WriteLine($"That did not go well: {e.Message}");
                }

                await FetchBasket();
                Console.Write("What thing are you interested in? (Apple|Pear|key): ");
            }
        }

        private static async Task FetchBasket()
        {
            var totalItemsProjection = new ProjectionFromBasket<CurrentThings>(CurrentThings.Replay);
            var currentState = await totalItemsProjection.Project("kitchen-basket");
            
            Console.WriteLine($"There are currently {currentState.NumberOfItems} things in your basket. Total weight {currentState.TotalWeight} kg.");
            Console.WriteLine($"Apples: {string.Join(", ", currentState.Apples)}");
            Console.WriteLine($"Pears:  {string.Join(", ", currentState.Pears)}");
            Console.WriteLine($"Keys:   {string.Join(", ", currentState.Keys)}");
            Console.WriteLine(Environment.NewLine);
        }
    }
}
