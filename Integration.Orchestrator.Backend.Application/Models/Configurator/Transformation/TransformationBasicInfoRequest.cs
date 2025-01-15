using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Connection
{
    [ExcludeFromCodeCoverage]
    public class TransformationBasicInfoRequest<T>
    {
        public T TransformationRequest { get; set; }

        public TransformationBasicInfoRequest(T transformationRequest) 
        {
            TransformationRequest = transformationRequest;
        }
     
    }
}
