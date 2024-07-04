namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueUpdateResponse : ModelResponse<ValueUpdate>
    {
    }
    public class ValueUpdate()
    {
        public Guid Id { get; set; }
    }
}
