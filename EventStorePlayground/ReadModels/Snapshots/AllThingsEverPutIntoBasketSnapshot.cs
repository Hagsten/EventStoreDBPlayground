namespace EventStorePlayground.ReadModels
{
    public class AllThingsEverPutIntoBasketSnapshot
    {
        public int NumberOfApples { get; set; }
        public int NumberOfPears { get; set; }
        public int NumberOfKeys { get; set; }
        public int NumberOfPostIts { get; set; }
        public int NumberOfUnknown { get; set; }
        public int NumberOfItems { get; set; }
        public string Id { get; set; }

        public static AllThingsEverPutIntoBasketSnapshot FromModel(AllThingsEverPutIntoBasket model)
        {
            return new AllThingsEverPutIntoBasketSnapshot
            {
                Id = model.Id,
                NumberOfApples = model.NumberOfApples,
                NumberOfItems = model.NumberOfItems,
                NumberOfKeys = model.NumberOfKeys,
                NumberOfPears = model.NumberOfPears,
                NumberOfPostIts = model.NumberOfPostIts,
                NumberOfUnknown = model.NumberOfUnknown
            };
        }
    }
}
