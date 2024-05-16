using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class IntegrationException : Exception
    {
        public IntegrationException() { }

        public IntegrationException(string message)
            : base(message) { }

        public IntegrationException(string message, Exception inner)
            : base(message, inner) { }
    }
}
