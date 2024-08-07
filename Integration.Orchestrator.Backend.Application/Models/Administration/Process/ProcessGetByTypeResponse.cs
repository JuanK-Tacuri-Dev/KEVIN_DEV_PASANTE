namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessGetByTypeResponse : ModelResponse<IEnumerable<ProcessGetByType>>
    {
    }
    public class ProcessGetByType : ProcessRequest
    {
        public Guid Id { get; set; }
    }
}
