using EventStorePlayground.Domains;
using EventStorePlayground.Domains.Basket.Events;
using System.Collections.Generic;
using System.Linq;

namespace EventStorePlayground.ReadModels
{
    public class CurrentThings
    {
        public decimal TotalWeight => _weights.Sum(x => x.Value);
        public int NumberOfItems => _things.Sum(x => x.Value.Count);
        public IReadOnlyCollection<string> Apples => _things["apples"];
        public IReadOnlyCollection<string> Pears => _things["pears"];
        public IReadOnlyCollection<string> Keys => _things["keys"];

        private Dictionary<string, List<string>> _things = new Dictionary<string, List<string>> {
            {"apples", new List<string>() },
            {"pears", new List<string>() },
            {"keys", new List<string>() }
        };

        private Dictionary<string, decimal> _weights = new Dictionary<string, decimal> { { "_", 0 } };

        private CurrentThings()
        { }

        public static CurrentThings Replay(ICollection<IDomainEvent> events)
        {
            var model = new CurrentThings();

            foreach (var e in events)
            {
                model.Apply((dynamic)e);
            }

            return model;
        }

        private void Apply(AppleAddedEvent e)
        {
            _things["apples"].Add(e.Id);
            _weights.Add(e.Id, e.Weight);
        }

        private void Apply(PearAddedEvent e)
        {
            _things["pears"].Add(e.Id);
            _weights.Add(e.Id, e.Weight);
        }

        private void Apply(KeyAddedEvent e)
        {
            _things["keys"].Add(e.Id);
            _weights.Add(e.Id, e.Weight);
        }

        private void Apply(ThingGrabbedEvent e)
        {
            foreach (var pair in _things)
            {
                var match = pair.Value.SingleOrDefault(x => x == e.Id);

                if(match is not null)
                {
                    pair.Value.Remove(match);
                    break;
                }
            }

            _weights.Remove(e.Id);
        }
    }
}
