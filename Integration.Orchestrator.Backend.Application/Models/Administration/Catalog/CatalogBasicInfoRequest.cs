namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
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
