namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus
{
    public class SynchronizationStatusDeleteResponse : ModelResponse<SynchronizationStatusDelete>
    {
    }
    public class SynchronizationStatusDelete
    {
        public Guid Id { get; set; }
    }
}
