namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryCreateResponse : ModelResponse<RepositoryCreate>
    {
    }
    public class RepositoryCreate()
    {
        public Guid Id { get; set; }
    }
}
