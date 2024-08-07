namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryGetByCodeResponse : ModelResponse<RepositoryGetByCode>
    {
    }
    public class RepositoryGetByCode : RepositoryRequest
    {
        public Guid Id { get; set; }
    }
}
