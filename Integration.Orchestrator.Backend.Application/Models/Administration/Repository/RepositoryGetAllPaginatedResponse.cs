namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryGetAllPaginatedResponse : ModelResponseGetAll<RepositoryGetAllRows> { }

    public class RepositoryGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<RepositoryGetAllPaginated> Rows { get; set; }
    }

    public class RepositoryGetAllPaginated : RepositoryRequest
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
    }
}
