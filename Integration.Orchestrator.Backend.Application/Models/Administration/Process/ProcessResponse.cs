namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TypeId { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid StatusId { get; set; }
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
        public Guid Id { get; set; }
        public Guid InternalStatusId { get; set; }
    }

    public class FilterResponse
    {
        public Guid PropertyId { get; set; }
        public Guid OperatorId { get; set; }
        public string Value { get; set; }
    }
}
