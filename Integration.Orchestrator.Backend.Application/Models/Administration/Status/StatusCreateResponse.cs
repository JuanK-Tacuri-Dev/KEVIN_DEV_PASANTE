namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
{
    public class StatusCreateResponse : ModelResponse<StatusCreate>
    {
    }
    public class StatusCreate()
    {
        public Guid Id { get; set; }
    }
}
