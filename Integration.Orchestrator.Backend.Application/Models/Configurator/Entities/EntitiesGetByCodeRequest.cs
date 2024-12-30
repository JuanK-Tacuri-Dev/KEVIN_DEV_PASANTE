using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesGetByCodeRequest
    {
        public string Code { get; set; } = string.Empty;
    }
}
