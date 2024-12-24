using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Commons
{
    [ExcludeFromCodeCoverage]
    public class PaginatedDefinition
    {
        public string Search { get; set; } = string.Empty;
        public int Sort_order { get; set; }
        public string Sort_field { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int First { get; set; }
        public bool activeOnly { get; set; }


    } 
}
