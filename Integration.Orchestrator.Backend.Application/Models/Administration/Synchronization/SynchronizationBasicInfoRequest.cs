namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
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
