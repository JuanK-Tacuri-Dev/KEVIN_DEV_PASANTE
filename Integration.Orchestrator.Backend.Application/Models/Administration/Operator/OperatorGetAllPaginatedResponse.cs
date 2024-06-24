namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<OperatorGetAllPaginated>>
    {
    }
    public class OperatorGetAllPaginated : OperatorRequest
    {
        public Guid Id { get; set; }
    }
}
