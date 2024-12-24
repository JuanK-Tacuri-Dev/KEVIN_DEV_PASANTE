using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusBasicInfoRequest<T>
    {
        public T StatusRequest { get; set; }

        public StatusBasicInfoRequest(T statusRequest) 
        {
            StatusRequest = statusRequest;
        }
     
    }
}
