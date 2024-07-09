namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class ConnectionDeleteResponse : ModelResponse<ConnectionDelete>
    {
    }
    public class ConnectionDelete
    {
        public Guid Id { get; set; }
    }
}
