﻿using EventStorePlayground.Data;
using EventStorePlayground.Domains;
using EventStorePlayground.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventStorePlayground.Projections
{
    public class ProjectionFromBasket<T> where T : class
    {
        private readonly Store _store;
        private readonly Func<ICollection<IDomainEvent>, T> replayer;

        public ProjectionFromBasket(Func<ICollection<IDomainEvent>, T> replayer)
        {
            _store = new Store();
            this.replayer = replayer;
        }

        public async Task<T> Project(string baskedId)
        {
            var eventStream = await _store.GetEventStream("basket", baskedId);

            var events = eventStream.Select(EventDeserializer.Deserialize).ToArray();

            return replayer(events);
        }
    }
}
