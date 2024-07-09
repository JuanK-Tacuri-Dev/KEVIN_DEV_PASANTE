namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionUpdateResponse : ModelResponse<ConnectionUpdate>
    {
    }
    public class ConnectionUpdate : ConnectionRequest
    {
        public Guid Id { get; set; }
    }
}
