namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    public class SynchronizationBasicInfoRequest<T>
    {
        public T SynchronizationRequest { get; set; }

        public SynchronizationBasicInfoRequest(T synchronizationRequest) 
        {
            SynchronizationRequest = synchronizationRequest;
        }
     
    }
}
