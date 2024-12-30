using Integration.Orchestrator.Backend.Domain.Commons;

namespace Integration.Orchestrator.Backend.Domain.Models
{
    public class PaginatedModel
    {
        public string Search { get; set; } = string.Empty;
        public SortOrdering Sort_order { get; set; }
        public IEnumerable<FilterModel> filter_Option { get; set; } = Enumerable.Empty<FilterModel>();
        public string Sort_field { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int First { get; set; }
        public bool activeOnly { get; set; }
    }

    public class FilterModel
    {
        public string filter_column { get; set; }
        public string[] filter_search { get; set; }
    }
}
