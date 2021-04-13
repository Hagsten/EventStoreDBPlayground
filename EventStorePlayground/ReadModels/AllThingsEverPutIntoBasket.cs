using EventStorePlayground.Domain.Events;
using EventStorePlayground.Domains;
using EventStorePlayground.Domains.Basket.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStorePlayground.ReadModels
{

    public class AllThingsEverPutIntoBasket
    {
        public int NumberOfApples { get; private set; }
        public int NumberOfPears { get; private set; }
        public int NumberOfKeys { get; private set; }
        public int NumberOfPostIts { get; private set; }
        public int NumberOfUnknown { get; private set; }

        public int NumberOfItems { get; private set; }
        public decimal TotalWeight => _weights.Sum(x => x.Value);

        private Dictionary<string, decimal> _weights = new Dictionary<string, decimal> { { "_", 0 } };

        private AllThingsEverPutIntoBasket()
        { }

        public static AllThingsEverPutIntoBasket Replay(ICollection<IDomainEvent> events)
        {
            var model = new AllThingsEverPutIntoBasket();

            foreach (var e in events.Where(x => x is ThingAddedEvent).Cast<ThingAddedEvent>())
            {
                model.Apply(e);
            }

            return model;
        }

        private void Apply(ThingAddedEvent e)
        {
            _weights.Add(e.Id, e.Weight);

            Action strategy = e switch
            {
                AppleAddedEvent => () => NumberOfApples++,
                PearAddedEvent => () => NumberOfPears++,
                KeyAddedEvent => () => NumberOfKeys++,
                _ => () => NumberOfUnknown++
            };

            strategy();

            NumberOfItems++;
        }

    }
}
