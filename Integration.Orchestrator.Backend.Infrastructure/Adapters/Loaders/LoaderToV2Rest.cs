using Integration.Orchestrator.Backend.Domain.Ports;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Loader
{
    [ExcludeFromCodeCoverage]
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
