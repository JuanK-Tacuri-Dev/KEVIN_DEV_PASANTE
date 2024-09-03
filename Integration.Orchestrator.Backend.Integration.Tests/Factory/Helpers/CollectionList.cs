namespace Integration.Orchestrator.Backend.Integration.Tests.Factory.Helpers
{
    public class CollectionList
    {
        public List<string> Collections { get; }

        public CollectionList(List<string> collections)
        {
            Collections = collections ?? throw new ArgumentNullException(nameof(collections));
        }
    }
}
