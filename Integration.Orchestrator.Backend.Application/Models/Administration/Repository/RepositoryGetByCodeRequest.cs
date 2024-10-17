using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryGetByCodeRequest
    {
        public string Code { get; set; }
    }
}
