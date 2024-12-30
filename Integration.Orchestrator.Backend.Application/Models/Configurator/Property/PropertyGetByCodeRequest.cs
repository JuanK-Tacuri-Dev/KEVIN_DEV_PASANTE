using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetByCodeRequest
    {
        public string Code { get; set; } = string.Empty;
    }
}
