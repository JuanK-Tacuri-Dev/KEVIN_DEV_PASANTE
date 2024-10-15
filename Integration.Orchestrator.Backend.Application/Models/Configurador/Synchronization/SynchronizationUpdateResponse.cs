namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    public class SynchronizationUpdateResponse : ModelResponse<SynchronizationUpdate>
    {
    }
    public class SynchronizationUpdate: SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
