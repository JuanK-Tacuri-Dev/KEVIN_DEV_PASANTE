using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionDeleteResponse : ModelResponse<ConnectionDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class ConnectionDelete
    {
        public Guid Id { get; set; }
    }
}
