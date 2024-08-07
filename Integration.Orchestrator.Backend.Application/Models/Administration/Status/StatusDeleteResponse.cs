namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
{
    public class StatusDeleteResponse : ModelResponse<StatusDelete>
    {
    }
    public class StatusDelete
    {
        public Guid Id { get; set; }
    }
}
