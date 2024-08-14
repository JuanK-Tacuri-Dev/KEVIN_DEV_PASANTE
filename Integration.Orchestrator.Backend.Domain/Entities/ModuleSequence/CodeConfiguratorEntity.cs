namespace Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence
{
    public class CodeConfiguratorEntity : Entity<Guid>
    {
        public int type { get; set; }
        public string value_text { get; set; }
        public int value_number { get; set; }
    }
}
