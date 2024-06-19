namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<ProcessGetAllPaginated>>
    {
    }
    public class ProcessGetAllPaginated : ProcessRequest
    {
        public Guid Id { get; set; }
    }
}
