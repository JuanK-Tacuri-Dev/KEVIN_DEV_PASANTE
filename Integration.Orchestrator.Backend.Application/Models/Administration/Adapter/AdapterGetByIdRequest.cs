using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
