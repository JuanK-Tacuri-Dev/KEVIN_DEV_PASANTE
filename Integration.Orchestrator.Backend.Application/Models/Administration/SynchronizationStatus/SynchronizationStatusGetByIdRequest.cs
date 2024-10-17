using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
