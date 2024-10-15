namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog
{
    public class CatalogRequest
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Detail { get; set; }
        public int? FatherCode { get; set; }
        public Guid StatusId { get; set; }
    }
}
