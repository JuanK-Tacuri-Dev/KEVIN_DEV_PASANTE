namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
{
    public class StatusGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<StatusGetAllPaginated>>
    {
    }
    public class StatusGetAllPaginated : StatusRequest
    {
        public Guid Id { get; set; }
    }
}
