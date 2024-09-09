namespace Integration.Orchestrator.Backend.Application.Commons
{
    public class PaginatedDefinition
    {
        public string Search { get; set; }
        public int Sort_order { get; set; }
        public string Sort_field { get; set; }
        public int Rows { get; set; }
        public int First { get; set; }


    } 
}
