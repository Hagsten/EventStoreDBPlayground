using EventStorePlayground.Domains;
using EventStorePlayground.Domains.Basket.Events;
using System.Collections.Generic;

namespace EventStorePlayground.ReadModels
{
    public class KeysInBasket : CountInBasket<KeyAddedEvent>
    {
        public IReadOnlyCollection<string> KeyIds => Ids;

        private KeysInBasket()
        { }

        public static KeysInBasket Replay(ICollection<IDomainEvent> events)
        {
            var model = new KeysInBasket();

            foreach (var e in events)
            {
                model.Apply((dynamic)e);
            }

            return model;
        }
    }
}
