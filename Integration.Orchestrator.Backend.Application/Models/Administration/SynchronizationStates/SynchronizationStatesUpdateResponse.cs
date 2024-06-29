namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates
{
    public class SynchronizationStatesUpdateResponse : ModelResponse<SynchronizationStatesUpdate>
    {
    }
    public class SynchronizationStatesUpdate()
    {
        public Guid Id { get; set; }
    }
}
