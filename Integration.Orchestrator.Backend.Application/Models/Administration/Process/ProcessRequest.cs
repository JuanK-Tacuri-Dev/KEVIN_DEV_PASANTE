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
        public Guid Id { get; set; }
        public List<PropertiesRequest> Properties { get; set; }
        public List<FilterRequest> Filters { get; set; }
    }

    public class PropertiesRequest
    {
        public Guid KeyId { get; set; }
    }

    public class FilterRequest
    {
        public Guid KeyId { get; set; }
        public Guid OperatorId { get; set; }
        public Guid ValueId { get; set; }
    }
}
