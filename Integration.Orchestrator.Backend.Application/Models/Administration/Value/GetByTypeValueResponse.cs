namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class GetByTypeValueResponse : ModelResponse<IEnumerable<GetByTypeValue>>
    {
    }
    public class GetByTypeValue : ValueRequest
    {
        public Guid Id { get; set; }
    }
}
