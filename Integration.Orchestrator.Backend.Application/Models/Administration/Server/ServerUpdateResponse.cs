namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    public class ServerUpdateResponse : ModelResponse<ServerUpdate>
    {
    }
    public class ServerUpdate : ServerRequest
    {
        public Guid Id { get; set; }
    }
}
