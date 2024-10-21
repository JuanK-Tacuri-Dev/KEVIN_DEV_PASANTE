using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorDetail
    {
        public string Params { get; set; }

        public string Message { get; set; }

        /*public ErrorDetail()
        {
            Params = new List<string>();
        }*/
    }
}
