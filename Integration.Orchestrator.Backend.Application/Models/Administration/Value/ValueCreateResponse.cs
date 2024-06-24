namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueCreateResponse : ModelResponse<ValueCreate>
    {
    }
    public class ValueCreate()
    {
        public Guid Id { get; set; }
    }
}
