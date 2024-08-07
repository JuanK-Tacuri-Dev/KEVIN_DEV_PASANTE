namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryDeleteResponse : ModelResponse<RepositoryDelete>
    {
    }
    public class RepositoryDelete
    {
        public Guid Id { get; set; }
    }
}
