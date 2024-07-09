namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryUpdateResponse : ModelResponse<RepositoryUpdate>
    {
    }
    public class RepositoryUpdate : RepositoryRequest
    {
        public Guid Id { get; set; }
    }
}
