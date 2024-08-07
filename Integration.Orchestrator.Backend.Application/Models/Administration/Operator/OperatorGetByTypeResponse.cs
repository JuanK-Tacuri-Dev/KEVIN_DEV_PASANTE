namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorGetByTypeResponse : ModelResponse<IEnumerable<OperatorGetByType>>
    {
    }
    public class OperatorGetByType : OperatorRequest
    {
        public Guid Id { get; set; }
    }
}
