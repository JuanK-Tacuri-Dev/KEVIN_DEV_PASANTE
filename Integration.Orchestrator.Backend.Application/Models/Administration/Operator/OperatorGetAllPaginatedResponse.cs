namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorGetAllPaginatedResponse : ModelResponseGetAll<OperatorGetAllRows> { }

    public class OperatorGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<OperatorGetAllPaginated> Rows { get; set; }
    }

    public class OperatorGetAllPaginated : OperatorResponse
    {
    }
}
