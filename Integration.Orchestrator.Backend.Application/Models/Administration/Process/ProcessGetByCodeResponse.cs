namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessGetByCodeResponse : ModelResponse<ProcessGetByCode>
    {
    }
    public class ProcessGetByCode : ProcessRequest
    {
        public Guid Id { get; set; }
    }
}
