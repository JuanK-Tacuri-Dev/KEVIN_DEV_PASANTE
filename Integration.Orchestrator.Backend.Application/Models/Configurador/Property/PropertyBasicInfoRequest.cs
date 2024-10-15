namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    public class PropertyBasicInfoRequest<T>
    {
        public T PropertyRequest { get; set; }

        public PropertyBasicInfoRequest(T propertyRequest) 
        {
            PropertyRequest = propertyRequest;
        }
     
    }
}
