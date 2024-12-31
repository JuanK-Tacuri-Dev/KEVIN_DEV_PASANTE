using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessDeleteResponse : ModelResponse<ProcessDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class ProcessDelete
    {
        public Guid Id { get; set;}
    }
}
