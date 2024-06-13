namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<ConnectionGetAllPaginated>>
    {
    }
    public class ConnectionGetAllPaginated : ConnectionRequest
    {
        public Guid Id { get; set; }
    }
}
