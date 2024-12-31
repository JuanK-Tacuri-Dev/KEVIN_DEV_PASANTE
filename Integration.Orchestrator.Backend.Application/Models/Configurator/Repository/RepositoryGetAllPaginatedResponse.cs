using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryGetAllPaginatedResponse : ModelResponseGetAll<RepositoryGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class RepositoryGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<RepositoryGetAllPaginated> Rows { get; set; } = Enumerable.Empty<RepositoryGetAllPaginated>();
    }

    [ExcludeFromCodeCoverage]
    public class RepositoryGetAllPaginated : RepositoryResponse
    {
    }
}
