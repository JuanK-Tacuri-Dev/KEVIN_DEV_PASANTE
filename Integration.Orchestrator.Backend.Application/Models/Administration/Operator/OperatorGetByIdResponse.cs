namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorGetByIdResponse : ModelResponse<OperatorGetById>
    {
    }
    public class OperatorGetById : OperatorRequest
    {
        public Guid Id { get; set; }
    }
}
