using Integration.Orchestrator.Backend.Integration.Tests.Factory;

namespace Integration.Orchestrator.Backend.Integration.Tests
{

    [CollectionDefinition("No Parallel Tests", DisableParallelization = true)]
    public class NoParallelTestsCollection : ICollectionFixture<CustomWebApplicationFactoryFixture>
    {
        // Esta clase no tiene código. Se usa solo para la definición de la colección.
    }
}
