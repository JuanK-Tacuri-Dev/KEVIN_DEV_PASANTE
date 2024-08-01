using Integration.Orchestrator.Backend.Application.Models;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public sealed class InvalidRequestException : Exception
    {
        
        private DetailsErrors DetailsError { get; set; }

        public DetailsErrors Details => DetailsError;

        public InvalidRequestException(string message, DetailsErrors details)
            : base(message)
        {
            DetailsError = details;
        }
    }

    public class DetailsErrors
    {
        public List<Dictionary<string, string>> Messages { get; set; }
        public object Data { get; set; }
    }

}
