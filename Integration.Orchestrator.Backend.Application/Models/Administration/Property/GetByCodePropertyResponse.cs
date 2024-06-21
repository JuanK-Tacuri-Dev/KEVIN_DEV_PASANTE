namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class GetByCodePropertyResponse : ModelResponse<GetByCodeProperty>
    {
    }
    public class GetByCodeProperty : PropertyRequest
    {
        public Guid Id { get; set; }
    }
}
