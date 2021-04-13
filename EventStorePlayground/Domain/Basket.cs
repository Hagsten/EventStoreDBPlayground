using EventStorePlayground.Domain.Events;
using System.Collections.Generic;
using System.Linq;

namespace EventStorePlayground.Domain
{
    public class Basket
    {
        public IReadOnlyCollection<IThing> Things => _things.AsReadOnly();
        private readonly List<IThing> _things;
        private readonly List<IDomainEvent> _events;

        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        public decimal TotalWeight => _things.Select(x => x.Weight).DefaultIfEmpty(0M).Sum();

        public Basket()
        {
            _things = new List<IThing>();
            _events = new List<IDomainEvent>();
        }

        public static Basket Replay(ICollection<IDomainEvent> events)
        {
            var basket = new Basket();

            foreach (var e in events)
            {
                basket.Apply((dynamic)e);
            }

            return basket;
        }

        private void Apply(AppleAddedEvent e)
        {
            _things.Add(new Apple(e.Id, e.Weight, e.FruitCondition));
        }

        private void Apply(PearAddedEvent e)
        {
            _things.Add(new Pear(e.Id, e.Weight, e.FruitCondition));
        }
   
        private void Apply(ThingGrabbedEvent e)
        {
            var theThing = _things.SingleOrDefault(x => x.Id == e.Id);

            _things.Remove(theThing);
        }

        private void Apply(KeyAddedEvent e)
        {
            _things.Add(new Key(e.Id, e.Weight, e.Owner));
        }

        internal void AddFruit(IFruit fruit)
        {
            IDomainEvent ev;

            if (fruit is Apple)
            {
                ev = new AppleAddedEvent(fruit.Id, fruit.Weight, fruit.FruitCondition);
            }
            else if (fruit is Pear)
            {
                ev = new PearAddedEvent(fruit.Id, fruit.Weight, fruit.FruitCondition);
            }
            else
            {
                ev = new UnknownFruitAddedEvent(fruit.Id, fruit.Weight, fruit.FruitCondition);
            }

            _events.Add(ev);

            Apply((dynamic)ev);
        }

        internal void AddKey(Key key)
        {
            var ev = new KeyAddedEvent(key.Id, key.Weight, key.Owner);

            _events.Add(ev);

            Apply((dynamic)ev);
        }

        internal void GrabAThing(string thingId)
        {
            var theThing = _things.SingleOrDefault(x => x.Id == thingId);

            if (theThing is null)
            {
                throw new System.Exception("no no, someone already took it out");
            }

            var ev = theThing is IFruit ? new FruitGrabbedEvent(thingId) : new ThingGrabbedEvent(thingId);

            _events.Add(ev);

            Apply((dynamic)ev);
        }
    }
}
