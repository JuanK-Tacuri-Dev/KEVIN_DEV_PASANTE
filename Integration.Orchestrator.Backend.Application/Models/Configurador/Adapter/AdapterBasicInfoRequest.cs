namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Adapter
{
    public class AdapterBasicInfoRequest<T>
    {
        public T AdapterRequest { get; set; }

        public AdapterBasicInfoRequest(T adapterRequest) 
        {
            AdapterRequest = adapterRequest;
        }
     
    }
}
