namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class OperatorEntity : Entity<Guid>
    {
        public string operator_name { get; set; }
        public string operator_code { get; set; }
        public Guid type_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        
    }
}
