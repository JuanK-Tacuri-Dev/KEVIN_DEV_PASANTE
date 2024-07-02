namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    public class ServerGetByCodeResponse : ModelResponse<ServerGetByCode>
    {
    }
    public class ServerGetByCode : ServerRequest
    {
        public Guid Id { get; set; }
    }
}
