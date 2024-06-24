namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<ValueGetAllPaginated>>
    {
    }
    public class ValueGetAllPaginated : ValueRequest
    {
        public Guid Id { get; set; }
    }
}
