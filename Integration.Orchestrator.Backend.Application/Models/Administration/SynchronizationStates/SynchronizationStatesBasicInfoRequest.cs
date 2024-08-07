namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates
{
    public class SynchronizationStatesBasicInfoRequest<T>
    {
        public T SynchronizationStatesRequest { get; set; }

        public SynchronizationStatesBasicInfoRequest(T synchronizationRequest) 
        {
            SynchronizationStatesRequest = synchronizationRequest;
        }
     
    }
}
