using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusRequest
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public string Background { get; set; }
    }
  
}
