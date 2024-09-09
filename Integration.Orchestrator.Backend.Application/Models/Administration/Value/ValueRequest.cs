namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueRequest
    {
        public string Name { get; set; }
        public Guid TypeId { get; set; }
        public Guid StatusId { get; set; }
    }
}
