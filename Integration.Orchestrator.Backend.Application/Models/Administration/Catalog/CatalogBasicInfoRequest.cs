using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogBasicInfoRequest<T>
    {
        public T CatalogRequest { get; set; }

        public CatalogBasicInfoRequest(T catalogRequest) 
        {
            CatalogRequest = catalogRequest;
        }
     
    }
}
