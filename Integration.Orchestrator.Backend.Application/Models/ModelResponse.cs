using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class ModelResponse<T> where T : class
    {
        public int Code { get; set; }

        public string[] Messages { get; set; } = new string[0];

        public required T Data { get; set; }

    }
}
