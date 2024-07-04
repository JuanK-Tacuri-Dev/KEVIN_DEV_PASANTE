namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessGetByIdResponse : ModelResponse<ProcessGetById>
    {
    }
    public class ProcessGetById : ProcessRequest
    {
        public Guid Id { get; set; }
    }
}
