using EventStorePlayground.Data;
using EventStorePlayground.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventStorePlayground.Projections
{
    public class ProjectionFromFruit<T> where T : class
    {
        private readonly Store _store;
        private readonly Func<ICollection<IDomainEvent>, T> replayer;

        public ProjectionFromFruit(Func<ICollection<IDomainEvent>, T> replayer)
        {
            _store = new Store();
            this.replayer = replayer;
        }

        public async Task<T> Project(string fruitId)
        {
            var eventStream = await _store.GetEventStream("fruit", fruitId);

            var events = eventStream.Select(EventDeserializer.Deserialize).ToArray();

            if (!events.Any())
            {
                return null;
            }

            return replayer(events);
        }
    }
}
