namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogRequest
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid? FatherId { get; set; }
        public bool IsFather { get; set; } = false;
        public string Detail { get; set; }
        public Guid StatusId { get; set; }
    }
}
