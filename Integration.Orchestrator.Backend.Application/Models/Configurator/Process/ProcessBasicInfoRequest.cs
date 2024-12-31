using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessBasicInfoRequest<T>
    {
        public T ProcessRequest { get; set; }

        public ProcessBasicInfoRequest(T processRequest) 
        {
            ProcessRequest = processRequest;
        }
     
    }
}
