namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid TypeId { get; set; }
        public Guid RepositoryId { get; set; }
    }
}
