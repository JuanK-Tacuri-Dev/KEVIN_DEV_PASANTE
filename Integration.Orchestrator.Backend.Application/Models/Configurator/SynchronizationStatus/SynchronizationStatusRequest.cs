using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusRequest
    {
        public string Key { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
    }
  
}
