namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    public class SynchronizationCreateResponse : ModelResponse<SynchronizationCreate>
    {
    }
    public class SynchronizationCreate: SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
