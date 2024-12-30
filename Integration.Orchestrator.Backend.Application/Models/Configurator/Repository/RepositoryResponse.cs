using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int? Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string AuthTypeName { get; set; } = string.Empty;
        public Guid? AuthTypeId { get; set; }
        public Guid StatusId { get; set; }
    }
}
