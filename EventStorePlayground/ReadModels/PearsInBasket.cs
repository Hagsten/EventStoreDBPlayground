using EventStorePlayground.Domains;
using EventStorePlayground.Domains.Basket.Events;
using System.Collections.Generic;

namespace EventStorePlayground.ReadModels
{
    public class PearsInBasket : CountInBasket<PearAddedEvent>
    {
        public IReadOnlyCollection<string> PearIds => Ids;

        private PearsInBasket()
        { }

        public static PearsInBasket Replay(ICollection<IDomainEvent> events)
        {
            var model = new PearsInBasket();

            foreach (var e in events)
            {
                model.Apply((dynamic)e);
            }

            return model;
        }       
    }
}
