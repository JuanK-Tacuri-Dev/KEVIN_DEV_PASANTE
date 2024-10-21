using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogResponse
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Detail { get; set; }
        public int? FatherCode { get; set; }
        public bool IsFather {  get; set; }
        public Guid StatusId { get; set; }
    }
}
