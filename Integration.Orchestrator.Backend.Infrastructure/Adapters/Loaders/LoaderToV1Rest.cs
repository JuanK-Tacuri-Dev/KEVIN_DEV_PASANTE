using Integration.Orchestrator.Backend.Domain.Entities.V2ToV1;
using Integration.Orchestrator.Backend.Domain.Ports;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Loader
{
    public class LoaderToV1Rest : ILoader<TestEntity>
    {
        public LoaderToV1Rest()
        {

        }
        public Task execute(IEnumerable<TestEntity> data)
        {
            return Task.FromResult(0);
        }
    }
}
