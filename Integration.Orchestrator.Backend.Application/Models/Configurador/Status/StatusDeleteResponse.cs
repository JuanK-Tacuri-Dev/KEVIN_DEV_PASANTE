namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Status
{
    public class StatusDeleteResponse : ModelResponse<StatusDelete>
    {
    }
    public class StatusDelete
    {
        public Guid Id { get; set; }
    }
}
