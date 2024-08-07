namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
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
