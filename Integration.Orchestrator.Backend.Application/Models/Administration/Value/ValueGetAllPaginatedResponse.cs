namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueGetAllPaginatedResponse : ModelResponseGetAll<ValueGetAllRows> { }

    public class ValueGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<ValueGetAllPaginated> Rows { get; set; }
    }

    public class ValueGetAllPaginated : ValueResponse
    {
    }
}
