using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusRequest
    {
        public string Key { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
    }
}
