using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationBasicInfoRequest<T>
    {
        public T IntegrationRequest { get; set; }

        public IntegrationBasicInfoRequest(T integrationRequest) 
        {
            IntegrationRequest = integrationRequest;
        }
     
    }
}
