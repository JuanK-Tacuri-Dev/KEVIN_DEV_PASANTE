using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationRequest
    {
        public string Name { get; set; }
        public Guid StatusId { get; set; }
        public string Observations { get; set; }
        public Guid UserId { get; set; }
        public List<ProcessRequest> Process { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProcessRequest
    {
        public Guid Id { get; set; }

    }
}
