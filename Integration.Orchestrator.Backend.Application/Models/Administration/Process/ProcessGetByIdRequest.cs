using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
