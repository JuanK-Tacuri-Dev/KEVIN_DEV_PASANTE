namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    public class ServerGetByIdResponse : ModelResponse<ServerGetById>
    {
    }
    public class ServerGetById : ServerRequest
    {
        public Guid Id { get; set; }
    }
}
