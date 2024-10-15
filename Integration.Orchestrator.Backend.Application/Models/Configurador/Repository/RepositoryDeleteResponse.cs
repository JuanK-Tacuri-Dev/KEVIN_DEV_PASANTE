namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Repository
{
    public class RepositoryDeleteResponse : ModelResponse<RepositoryDelete>
    {
    }
    public class RepositoryDelete
    {
        public Guid Id { get; set; }
    }
}
