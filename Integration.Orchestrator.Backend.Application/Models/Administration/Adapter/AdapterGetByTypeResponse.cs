namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    public class AdapterGetByTypeResponse : ModelResponse<IEnumerable<AdapterGetByType>>
    {
    }
    public class AdapterGetByType : AdapterRequest
    {
        public Guid Id { get; set; }
    }
}
