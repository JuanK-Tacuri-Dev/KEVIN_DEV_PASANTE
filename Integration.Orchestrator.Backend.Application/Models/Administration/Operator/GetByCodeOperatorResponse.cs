namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class GetByCodeOperatorResponse : ModelResponse<GetByCodeOperator>
    {
    }
    public class GetByCodeOperator : OperatorRequest
    {
        public Guid Id { get; set; }
    }
}
