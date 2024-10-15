namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog
{
    public class CatalogBasicInfoRequest<T>
    {
        public T CatalogRequest { get; set; }

        public CatalogBasicInfoRequest(T catalogRequest) 
        {
            CatalogRequest = catalogRequest;
        }
     
    }
}
