namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class GetByTypePropertyResponse : ModelResponse<IEnumerable<GetByTypeProperty>>
    {
    }
    public class GetByTypeProperty : PropertyRequest
    {
        public Guid Id { get; set; }
    }
}
