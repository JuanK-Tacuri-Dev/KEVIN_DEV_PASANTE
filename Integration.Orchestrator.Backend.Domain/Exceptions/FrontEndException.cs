using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class FrontEndException : Exception
    {
        public FrontEndException() { }

        public FrontEndException(string message)
            : base(message) { }

        public FrontEndException(string message, Exception inner)
            : base(message, inner) { }
    }
}
