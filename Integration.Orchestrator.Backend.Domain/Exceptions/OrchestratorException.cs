using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class OrchestratorException : Exception
    {
        public OrchestratorException() { }

        public OrchestratorException(string message)
            : base(message) { }

        public OrchestratorException(string message, Exception inner)
            : base(message, inner) { }
    }
}
