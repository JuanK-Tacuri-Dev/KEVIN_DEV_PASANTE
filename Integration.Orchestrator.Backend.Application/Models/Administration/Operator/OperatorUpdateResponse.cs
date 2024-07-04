namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorUpdateResponse : ModelResponse<OperatorUpdate>
    {
    }
    public class OperatorUpdate()
    {
        public Guid Id { get; set; }
    }
}
