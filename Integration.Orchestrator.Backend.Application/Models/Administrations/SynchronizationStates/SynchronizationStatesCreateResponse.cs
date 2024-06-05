namespace Integration.Orchestrator.Backend.Application.Models.Administrations.SynchronizationStates
{
    public class SynchronizationStatesCreateResponse : ModelResponse<SynchronizationStatesCreate>
    {
    }
    public class SynchronizationStatesCreate()
    {
        public Guid Id { get; set; }
    }
}
