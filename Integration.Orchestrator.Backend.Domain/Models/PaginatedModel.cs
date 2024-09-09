using Integration.Orchestrator.Backend.Domain.Commons;

namespace Integration.Orchestrator.Backend.Domain.Models
{
    public class PaginatedModel
    {
        public string Search { get; set; }
        public SortOrdering Sort_order { get; set; }
        public string Sort_field { get; set; }
        public int Rows { get; set; }
        public int First { get; set; }
    }
}
