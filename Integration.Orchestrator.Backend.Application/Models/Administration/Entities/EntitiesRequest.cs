namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesRequest
    {
        public string Name { get; set; }
        public Guid TypeId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid StatusId { get; set; }
    }
}
