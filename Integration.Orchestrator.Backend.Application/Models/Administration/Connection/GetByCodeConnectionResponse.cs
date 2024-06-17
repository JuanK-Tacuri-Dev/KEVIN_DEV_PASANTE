namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class GetByCodeConnectionResponse : ModelResponse<GetByCodeConnection>
    {
    }
    public class GetByCodeConnection : ConnectionRequest
    {
        public Guid Id { get; set; }
    }
}
