namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    public class ServerCreateResponse : ModelResponse<ServerCreate>
    {
    }
    public class ServerCreate : ServerRequest
    {
        public Guid Id { get; set; }
    }
}
