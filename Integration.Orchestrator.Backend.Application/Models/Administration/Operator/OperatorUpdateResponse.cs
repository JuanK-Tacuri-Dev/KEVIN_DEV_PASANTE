namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorUpdateResponse : ModelResponse<OperatorUpdate>
    {
    }
    public class OperatorUpdate : OperatorRequest
    {
        public Guid Id { get; set; }
    }
}
