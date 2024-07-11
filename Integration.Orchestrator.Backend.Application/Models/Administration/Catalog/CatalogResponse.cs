namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public Guid FatherId { get; set; }
        public Guid StatusId { get; set; }
    }
}
