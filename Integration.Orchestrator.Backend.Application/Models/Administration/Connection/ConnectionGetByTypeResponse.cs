namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionGetByTypeResponse : ModelResponse<IEnumerable<ConnectionGetByType>>
    {
    }
    public class ConnectionGetByType : ConnectionRequest
    {
        public Guid Id { get; set; }
    }
}
