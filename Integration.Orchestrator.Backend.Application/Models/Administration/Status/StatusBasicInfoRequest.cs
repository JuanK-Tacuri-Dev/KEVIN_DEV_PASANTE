using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
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
