namespace Integration.Orchestrator.Backend.Domain.Models
{
    public class PaginatedModel
    {
        public string Search { get; set; }
        public int SortOrder { get; set; }
        public string SortBy { get; set; }
        public int Rows { get; set; }
        public int Page { get; set; }
    }
}
