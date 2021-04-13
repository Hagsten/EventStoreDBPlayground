using EventStorePlayground.Domains;
using EventStorePlayground.Domains.Fruit.Events;
using System.Collections.Generic;

namespace EventStorePlayground.ReadModels
{
    public class IsFruitEatable
    {
        public bool Value { get; private set;  }

        public IsFruitEatable()
        {
            Value = true;
        }

        public static IsFruitEatable Replay(ICollection<IDomainEvent> events)
        {
            var model = new IsFruitEatable();

            foreach (var e in events)
            {
                model.Apply(e);
            }

            return model;
        }

        private void Apply(IDomainEvent e)
        {
            Value = e switch
            {
                FruitDecomposedEvent => false,
                _ => Value
            };
        }
    }
}
