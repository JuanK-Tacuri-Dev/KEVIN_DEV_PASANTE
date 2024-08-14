namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryGetByIdResponse : ModelResponse<RepositoryGetById>
    {
    }
    public class RepositoryGetById : RepositoryRequest
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
    }
}
