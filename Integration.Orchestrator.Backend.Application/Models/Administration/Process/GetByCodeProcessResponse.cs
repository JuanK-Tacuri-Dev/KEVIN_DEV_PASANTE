namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class GetByCodeProcessResponse : ModelResponse<GetByCodeProcess>
    {
    }
    public class GetByCodeProcess : ProcessRequest
    {
        public Guid Id { get; set; }
    }
}
