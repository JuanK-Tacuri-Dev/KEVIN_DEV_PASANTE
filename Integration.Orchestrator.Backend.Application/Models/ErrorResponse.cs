using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponse
    {
        public int Code { get; set; }

        public string[] Messages { get; set; }

    }
}
