using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public sealed class InvalidRequestException(string message, DetailsErrors details) : Exception(message)
    {

        private DetailsErrors DetailsError { get; } = details;

        public DetailsErrors Details => DetailsError;
    }

    public class DetailsErrors
    {
        public List<Dictionary<string, string>> Messages { get; set; }
        public object Data { get; set; }
    }

}
