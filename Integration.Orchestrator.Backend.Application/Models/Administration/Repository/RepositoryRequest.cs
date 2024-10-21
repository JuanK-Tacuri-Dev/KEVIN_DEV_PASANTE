using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryRequest
    {
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public Guid? AuthTypeId { get; set; }
        public Guid StatusId { get; set; }
    }
}
