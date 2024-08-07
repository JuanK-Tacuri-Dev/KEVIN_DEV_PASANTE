namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorDeleteResponse : ModelResponse<OperatorDelete>
    {
    }
    public class OperatorDelete
    {
        public Guid Id { get; set; }
    }
}
