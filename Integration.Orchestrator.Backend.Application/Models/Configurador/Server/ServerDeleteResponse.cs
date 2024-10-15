namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    public class ServerDeleteResponse : ModelResponse<ServerDelete>
    {
    }
    public class ServerDelete
    {
        public Guid Id { get; set; }
    }
}
