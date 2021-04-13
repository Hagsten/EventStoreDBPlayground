//using EventStorePlayground.ReadModels;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EventStorePlayground.Projections
//{
//    public class AllThingsEverPutIntoBasketProjection
//    {
//        private readonly Store _store;

//        public AllThingsEverPutIntoBasketProjection()
//        {
//            _store = new Store();
//        }

//        public async Task<AllThingsEverPutIntoBasket> Project(string baskedId)
//        {
//            var eventStream = await _store.GetEventStream("basket", baskedId);

//            var events = eventStream.Select(EventDeserializer.Deserialize).ToArray();

//            return AllThingsEverPutIntoBasket.Replay(events);
//        }
//    }
//}
