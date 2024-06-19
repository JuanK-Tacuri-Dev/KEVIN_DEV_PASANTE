namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessCreateResponse : ModelResponse<ProcessCreate>
    {
    }
    public class ProcessCreate()
    {
        public Guid Id { get; set; }
    }
}
