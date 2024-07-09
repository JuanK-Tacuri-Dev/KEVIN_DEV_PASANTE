namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessResponse
    {
        public Guid Id { get; set; }
        public string ProcessCode { get; set; }
        public string Type { get; set; }
        public Guid ConnectionId { get; set; }
        public List<EntitiesResponse> Entities { get; set; }
    }

    public class EntitiesResponse
    {
        public Guid Id { get; set; }
        public List<PropertiesResponse> Properties { get; set; }
        public List<FilterResponse> Filters { get; set; }
    }

    public class PropertiesResponse
    {
        public Guid KeyId { get; set; }
    }

    public class FilterResponse
    {
        public Guid KeyId { get; set; }
        public Guid OperatorId { get; set; }
        public Guid ValueId { get; set; }
    }
}
