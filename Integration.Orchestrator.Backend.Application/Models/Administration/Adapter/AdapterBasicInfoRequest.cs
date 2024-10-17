using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterBasicInfoRequest<T>
    {
        public T AdapterRequest { get; set; }

        public AdapterBasicInfoRequest(T adapterRequest) 
        {
            AdapterRequest = adapterRequest;
        }
     
    }
}
