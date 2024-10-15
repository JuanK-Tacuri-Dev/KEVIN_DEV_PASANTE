namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Connection
{
    public class ConnectionDeleteResponse : ModelResponse<ConnectionDelete>
    {
    }
    public class ConnectionDelete
    {
        public Guid Id { get; set; }
    }
}
