namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class GetByTypeOperatorResponse : ModelResponse<IEnumerable<GetByTypeOperator>>
    {
    }
    public class GetByTypeOperator : OperatorRequest
    {
        public Guid Id { get; set; }
    }
}
