using EventStorePlayground.Domain.Events;
using EventStorePlayground.Domains;
using EventStorePlayground.Domains.Basket.Events;
using System.Collections.Generic;
using System.Linq;

namespace EventStorePlayground.ReadModels
{
    public abstract class CountInBasket<TEvent> where TEvent : IDomainEvent
    {
        private List<string> _ids = new List<string>();

        protected IReadOnlyCollection<string> Ids => _ids.AsReadOnly();
       
        protected void Apply(ThingAddedEvent e)
        {
            if (e is TEvent)
            {
                _ids.Add(e.Id);
            }
        }

        protected void Apply(ThingGrabbedEvent e)
        {
            var id = _ids.SingleOrDefault(x => x == e.Id);

            if (id is not null)
            {
                _ids.Remove(id);
            }
        }
    }
}
