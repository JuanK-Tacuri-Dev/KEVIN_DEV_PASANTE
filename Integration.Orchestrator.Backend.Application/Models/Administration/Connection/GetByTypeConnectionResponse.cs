namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    public class GetByTypeConnectionResponse : ModelResponse<IEnumerable<GetByTypeConnection>>
    {
    }
    public class GetByTypeConnection : ConnectionRequest
    {
        public Guid Id { get; set; }
    }
}
