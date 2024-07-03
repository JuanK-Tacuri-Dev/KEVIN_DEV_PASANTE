namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessRequest
    {
        public string ProcessCode { get; set; }
        public string Type { get; set; }
        public Guid ConnectionId { get; set; }
        public List<EntitiesRequest> Entities { get; set; }
    }

    public class EntitiesRequest
    {
        public string Name { get; set; }
        public List<FilterRequest> Filters { get; set; }
    }

    public class FilterRequest
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
