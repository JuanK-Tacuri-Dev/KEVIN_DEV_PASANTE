namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates
{
    public class SynchronizationStatesDeleteResponse : ModelResponse<SynchronizationStatesDelete>
    {
    }
    public class SynchronizationStatesDelete
    {
        public Guid Id { get; set; }
    }
}
