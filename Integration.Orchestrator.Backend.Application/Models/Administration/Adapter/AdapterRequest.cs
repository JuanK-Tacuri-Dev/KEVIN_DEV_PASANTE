namespace Integration.Orchestrator.Backend.Application.Models.Administration.Adapter
{
    public class AdapterRequest
    {
        public string Name { get; set; }
        public Guid TypeAdapterId { get; set; }
        public string Version { get; set; }
        public Guid StatusId { get; set; }
    }
}
