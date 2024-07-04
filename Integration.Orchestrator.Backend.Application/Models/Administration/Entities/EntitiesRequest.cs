namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public Guid IdServer { get; set; }
    }
}
