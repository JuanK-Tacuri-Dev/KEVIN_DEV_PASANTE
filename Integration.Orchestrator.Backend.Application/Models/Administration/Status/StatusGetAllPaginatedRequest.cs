using Integration.Orchestrator.Backend.Application.Commons;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusGetAllPaginatedRequest : PaginatedDefinition
    {
        public bool ActiveOnly { get; set; }
    }
}