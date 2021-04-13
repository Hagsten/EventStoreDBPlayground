using EventStorePlayground.Domains;
using EventStorePlayground.Domains.Basket.Events;
using System.Collections.Generic;

namespace EventStorePlayground.ReadModels
{
    public class ApplesInBasket : CountInBasket<AppleAddedEvent>
    {
        public IReadOnlyCollection<string> AppleIds => Ids;

        private ApplesInBasket()
        { }

        public static ApplesInBasket Replay(ICollection<IDomainEvent> events)
        {
            var model = new ApplesInBasket();

            foreach (var e in events)
            {
                model.Apply((dynamic)e);
            }

            return model;
        }      
    }
}
