using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Transformation
{
    [ExcludeFromCodeCoverage]
    public class TransformationResponse
    {

        public Guid Id { get; set; }
        public string code { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        
    }
}
