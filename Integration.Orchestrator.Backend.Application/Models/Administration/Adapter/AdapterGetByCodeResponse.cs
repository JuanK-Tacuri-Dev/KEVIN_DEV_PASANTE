namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    public class AdapterGetByCodeResponse : ModelResponse<AdapterGetByCode>
    {
    }
    public class AdapterGetByCode : AdapterRequest
    {
        public Guid Id { get; set; }
    }
}
