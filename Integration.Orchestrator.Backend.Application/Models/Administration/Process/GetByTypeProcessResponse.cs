namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class GetByTypeProcessResponse : ModelResponse<IEnumerable<GetByTypeProcess>>
    {
    }
    public class GetByTypeProcess : ProcessRequest
    {
        public Guid Id { get; set; }
    }
}
