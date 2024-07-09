namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates
{
    public class SynchronizationStatesUpdateResponse : ModelResponse<SynchronizationStatesUpdate>
    {
    }
    public class SynchronizationStatesUpdate : SynchronizationStatesRequest
    {
        public Guid Id { get; set; }
    }
}
