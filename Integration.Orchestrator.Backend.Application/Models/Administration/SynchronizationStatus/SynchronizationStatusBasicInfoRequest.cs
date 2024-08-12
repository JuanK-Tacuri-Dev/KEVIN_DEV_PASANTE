namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus
{
    public class SynchronizationStatusBasicInfoRequest<T>
    {
        public T SynchronizationStatesRequest { get; set; }

        public SynchronizationStatusBasicInfoRequest(T synchronizationRequest) 
        {
            SynchronizationStatesRequest = synchronizationRequest;
        }
     
    }
}
