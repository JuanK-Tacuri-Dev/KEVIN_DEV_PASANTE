using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class ModelResponse<T> where T : class
    {
        public int Code { get; set; }

        public string[] Messages { get; set; }

        public T Data { get; set; }

    }
}
