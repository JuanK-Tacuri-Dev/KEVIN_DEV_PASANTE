namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    public class ServerGetByTypeResponse : ModelResponse<IEnumerable<ServerGetByType>>
    {
    }
    public class ServerGetByType : ServerRequest
    {
        public Guid Id { get; set; }
    }
}
