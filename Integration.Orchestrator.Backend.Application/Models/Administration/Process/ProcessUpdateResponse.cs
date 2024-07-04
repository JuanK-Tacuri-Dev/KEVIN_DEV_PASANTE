namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessUpdateResponse : ModelResponse<ProcessUpdate>
    {
    }
    public class ProcessUpdate()
    {
        public Guid Id { get; set; }
    }
}
