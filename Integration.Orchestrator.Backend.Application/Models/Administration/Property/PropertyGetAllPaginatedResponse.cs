namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyGetAllPaginatedResponse : ModelResponseGetAll<PropertyGetAllRows> { }

    public class PropertyGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<PropertyGetAllPaginated> Rows { get; set; }
    }

    public class PropertyGetAllPaginated : PropertyResponse
    {
    }
}
