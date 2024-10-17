using Integration.Orchestrator.Backend.Application.Commons;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusGetAllPaginatedRequest : PaginatedDefinition
    {
        public bool ActiveOnly { get; set; }
    }
}