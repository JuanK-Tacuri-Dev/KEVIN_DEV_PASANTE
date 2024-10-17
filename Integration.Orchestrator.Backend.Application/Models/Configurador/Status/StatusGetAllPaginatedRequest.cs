using Integration.Orchestrator.Backend.Application.Commons;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Status
{
    public class StatusGetAllPaginatedRequest : PaginatedDefinition
    {
        public bool ActiveOnly { get; set; }
    }
}