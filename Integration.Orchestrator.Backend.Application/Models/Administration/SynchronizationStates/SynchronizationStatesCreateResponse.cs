namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates
{
    public class SynchronizationStatesCreateResponse : ModelResponse<SynchronizationStatesCreate>
    {
    }
    public class SynchronizationStatesCreate : SynchronizationStatesRequest
    {
        public Guid Id { get; set; }
    }
}
