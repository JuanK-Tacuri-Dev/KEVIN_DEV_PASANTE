namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    public class AdapterGetAllPaginatedResponse : ModelResponseGetAll<AdapterGetAllRows> { }

    public class AdapterGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<AdapterGetAllPaginated> Rows { get; set; }
    }

    public class AdapterGetAllPaginated : AdapterResponse 
    { }
    
}
