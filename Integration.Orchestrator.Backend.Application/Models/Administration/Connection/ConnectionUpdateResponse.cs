namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionUpdateResponse : ModelResponse<ConnectionUpdate>
    {
    }
    public class ConnectionUpdate()
    {
        public Guid Id { get; set; }
    }
}
