namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
{
    public class StatusGetByIdResponse : ModelResponse<StatusGetById>
    {
    }
    public class StatusGetById : StatusRequest
    {
        public Guid Id { get; set; }
    }
}
