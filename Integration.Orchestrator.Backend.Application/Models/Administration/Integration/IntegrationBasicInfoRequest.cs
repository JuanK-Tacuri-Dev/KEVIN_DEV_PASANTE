namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationBasicInfoRequest<T>
    {
        public T IntegrationRequest { get; set; }

        public IntegrationBasicInfoRequest(T integrationRequest) 
        {
            IntegrationRequest = integrationRequest;
        }
     
    }
}
