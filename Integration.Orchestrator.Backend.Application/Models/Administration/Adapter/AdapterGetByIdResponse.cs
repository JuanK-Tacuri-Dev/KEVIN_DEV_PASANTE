namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    public class AdapterGetByIdResponse : ModelResponse<AdapterGetById>
    {
    }
    public class AdapterGetById : AdapterRequest
    {
        public Guid Id { get; set; }
    }
}
