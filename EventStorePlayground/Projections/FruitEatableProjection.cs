//using EventStorePlayground.Data;
//using EventStorePlayground.ReadModels;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EventStorePlayground.Projections
//{
//    public class FruitEatableProjection
//    {
//        private readonly Store _store;

//        public FruitEatableProjection()
//        {
//            _store = new Store();
//        }

//        public async Task<IsFruitEatable> Project(string fruitId)
//        {
//            var eventStream = await _store.GetEventStream("fruit", fruitId);

//            var events = eventStream.Select(EventDeserializer.Deserialize).ToArray();

//            return IsFruitEatable.Replay(events);
//        }
//    }
//}
