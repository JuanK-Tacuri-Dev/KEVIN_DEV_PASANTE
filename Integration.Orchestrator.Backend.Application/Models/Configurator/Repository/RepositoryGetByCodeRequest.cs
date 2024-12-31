using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryGetByCodeRequest
    {
        public string Code { get; set; } = string.Empty;
    }
}
