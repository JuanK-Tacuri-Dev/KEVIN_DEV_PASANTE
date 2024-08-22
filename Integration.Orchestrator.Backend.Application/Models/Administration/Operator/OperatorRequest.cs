namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid TypeId { get; set; }
    }
}
