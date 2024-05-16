using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InfrastructureException : Exception
    {
        public InfrastructureException() { }

        public InfrastructureException(string message)
            : base(message) { }

        public InfrastructureException(string message, Exception inner)
            : base(message, inner) { }
    }
}
