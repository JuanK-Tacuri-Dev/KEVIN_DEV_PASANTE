namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionGetByCodeResponse : ModelResponse<ConnectionGetByCode>
    {
    }
    public class ConnectionGetByCode : ConnectionRequest
    {
        public Guid Id { get; set; }
    }
}
