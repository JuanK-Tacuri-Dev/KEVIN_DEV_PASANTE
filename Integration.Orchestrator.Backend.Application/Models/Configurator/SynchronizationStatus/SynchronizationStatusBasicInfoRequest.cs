using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusBasicInfoRequest<T>
    {
        public T SynchronizationStatesRequest { get; set; }

        public SynchronizationStatusBasicInfoRequest(T synchronizationRequest) 
        {
            SynchronizationStatesRequest = synchronizationRequest;
        }
     
    }
}
