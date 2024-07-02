namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    public class ServerGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<ServerGetAllPaginated>>
    {
    }
    public class ServerGetAllPaginated : ServerRequest
    {
        public Guid Id { get; set; }
    }
}
