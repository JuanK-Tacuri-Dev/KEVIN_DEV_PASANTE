using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorDetail
    {
        public string Params { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

    }
}
