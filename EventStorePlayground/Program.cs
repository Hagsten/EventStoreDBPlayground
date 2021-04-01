using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventStorePlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello EventStoreDB!");

            DoIt().Wait();
        }

        private static async Task DoIt()
        {
            var settings = EventStoreClientSettings.Create("esdb://localhost:2113?tls=false&keepAliveInterval=-1&keepAliveTimeout=-1");

            var client = new EventStoreClient(settings);
            var lastUserEvent = await client.ReadStreamAsync(Direction.Backwards, "users-stream", StreamPosition.End, 1).ToListAsync();

            if(lastUserEvent.Count == 0)
            {
                await CreateUser(client);

                lastUserEvent = await client.ReadStreamAsync(Direction.Backwards, "users-stream", StreamPosition.End, 1).ToListAsync();
            }

            var entityId = JsonSerializer.Deserialize<UserAddedEvent>(lastUserEvent.Single().Event.Data.Span).EntityId;

            var addressProvidedEvent = new AddressProvidedEvent("The road 1337", "702 30", "Sweden");

            var addressProvidedEventData = new EventData(
                Uuid.NewUuid(),
                "AddressProvidedEvent",
                JsonSerializer.SerializeToUtf8Bytes(addressProvidedEvent)
            );

            await client.AppendToStreamAsync(
                $"address-stream-{entityId}",
                StreamState.Any,
                new[] { addressProvidedEventData });

            var user = await client.ReadStreamAsync(
                Direction.Forwards,
                $"user-stream-{entityId}",
                StreamPosition.Start).ToListAsync();

            var userAddress = await client.ReadStreamAsync(
                Direction.Forwards,
                $"address-stream-{entityId}",
                StreamPosition.Start).ToListAsync();

            var events = user.Concat(userAddress).OrderBy(x => x.Event.Created).Select(Deserialize).ToList();

            var model = User.Replay(events);
        }

        private static async Task CreateUser(EventStoreClient client)
        {
            var userEvent = new UserAddedEvent("Andreas", "Hagsten", true, Guid.NewGuid().ToString("N"));

            var userAddedEventData = new EventData(
                Uuid.NewUuid(),
                "UserAddedEvent",
                JsonSerializer.SerializeToUtf8Bytes(userEvent)
            );

            await client.AppendToStreamAsync(
                $"users-stream",
                StreamState.Any,
                new[] { userAddedEventData });

            await client.AppendToStreamAsync(
                $"user-stream-{userEvent.EntityId}",
                StreamState.Any,
                new[] { userAddedEventData });
        }

        private static object Deserialize(ResolvedEvent e)
        {
            return e.Event.EventType switch
            {
                "UserAddedEvent" => JsonSerializer.Deserialize<UserAddedEvent>(e.Event.Data.Span),
                "AddressProvidedEvent" => JsonSerializer.Deserialize<AddressProvidedEvent>(e.Event.Data.Span),
                _ => throw new NotImplementedException()
            };
        }
    }


    public interface IDomainEvent
    {
    }

    public class User
    {
        public string EntityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public bool Active { get; set; }

        public static User Replay(ICollection<object> events)
        {
            var user = new User();

            foreach (var e in events)
            {
                user.Apply((dynamic)e);
            }

            return user;
        }

        private void Apply(UserAddedEvent e)
        {
            FirstName = e.FirstName;
            LastName = e.LastName;
            Active = e.Active;
        }

        private void Apply(AddressProvidedEvent e)
        {
            Address = new Address
            {
                Country = e.Country,
                Street = e.Street,
                ZipCode = e.ZipCode
            };
        }

    }

    public class Address
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }

    public class UserAddedEvent : IDomainEvent
    {
        public UserAddedEvent(string firstName, string lastName, bool active, string entityId)
        {
            FirstName = firstName;
            LastName = lastName;
            Active = active;
            EntityId = entityId;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public bool Active { get; }
        public string EntityId { get; }
    }

    public class AddressProvidedEvent : IDomainEvent
    {
        public AddressProvidedEvent(string street, string zipCode, string country)
        {
            Street = street;
            ZipCode = zipCode;
            Country = country;
        }

        public string Street { get; }
        public string ZipCode { get; }
        public string Country { get; }
    }
}
