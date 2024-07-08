namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    public class AdapterCreateResponse : ModelResponse<AdapterCreate>
    {
    }
    public class AdapterCreate()
    {
        public Guid Id { get; set; }
    }
}
