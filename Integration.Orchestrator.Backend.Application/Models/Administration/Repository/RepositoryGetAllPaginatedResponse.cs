namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<RepositoryGetAllPaginated>>
    {
    }
    public class RepositoryGetAllPaginated : RepositoryRequest
    {
        public Guid Id { get; set; }
    }
}
