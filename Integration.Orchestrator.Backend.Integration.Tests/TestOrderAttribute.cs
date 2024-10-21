namespace Integration.Orchestrator.Backend.Integration.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestOrderAttribute : FactAttribute
    {
        public int Order { get; }

        public TestOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
