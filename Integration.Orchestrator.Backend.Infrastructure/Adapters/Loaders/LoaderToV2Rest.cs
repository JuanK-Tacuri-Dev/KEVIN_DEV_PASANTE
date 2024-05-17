using Integration.Orchestrator.Backend.Domain.Ports;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Loader
{
    public class LoaderToV2Rest : ILoader<string>
    {
        public LoaderToV2Rest()
        {

        }
        public Task execute(IEnumerable<string> data)
        {
            return Task.FromResult(0);
        }
    }
}
