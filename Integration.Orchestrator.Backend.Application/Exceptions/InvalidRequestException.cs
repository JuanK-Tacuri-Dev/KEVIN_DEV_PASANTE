using Integration.Orchestrator.Backend.Application.Models;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public sealed class InvalidRequestException : Exception
    {
        private List<ErrorDetail> DetailsError { get; set; }

        public List<ErrorDetail> Details => DetailsError;

        public InvalidRequestException(string message, List<ErrorDetail> details)
            : base(message)
        {
            DetailsError = details;
        }
    }
}
