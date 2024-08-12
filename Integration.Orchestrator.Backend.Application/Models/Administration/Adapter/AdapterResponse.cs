namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    public class AdapterResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid TypeAdapterId { get; set; }
        public string Version { get; set; }
        public Guid StatusId { get; set; }
    }
}
