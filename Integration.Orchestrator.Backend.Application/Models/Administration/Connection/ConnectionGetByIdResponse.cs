namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionGetByIdResponse : ModelResponse<ConnectionGetById>
    {
    }
    public class ConnectionGetById : ConnectionRequest
    {
        public Guid Id { get; set; }
    }
}
