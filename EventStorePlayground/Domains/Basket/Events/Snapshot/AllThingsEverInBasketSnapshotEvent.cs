using EventStorePlayground.ReadModels;

namespace EventStorePlayground.Domains.Basket.Events.Snapshot
{
    public class AllThingsEverInBasketSbapshotEvent : ISnapshotEvent
    {
        public AllThingsEverInBasketSbapshotEvent(AllThingsEverPutIntoBasketSnapshot snapshot)
        {
            Snapshot = snapshot;
        }

        public AllThingsEverPutIntoBasketSnapshot Snapshot { get; }
        public string Id => Snapshot.Id;
    }
}
