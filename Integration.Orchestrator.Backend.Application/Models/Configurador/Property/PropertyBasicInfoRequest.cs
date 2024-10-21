using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyBasicInfoRequest<T>
    {
        public T PropertyRequest { get; set; }

        public PropertyBasicInfoRequest(T propertyRequest) 
        {
            PropertyRequest = propertyRequest;
        }
     
    }
}
