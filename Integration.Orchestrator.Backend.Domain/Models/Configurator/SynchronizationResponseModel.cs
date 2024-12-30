namespace Integration.Orchestrator.Backend.Domain.Models.Configurator
{
    public class SynchronizationResponseModel
    {
        public Guid Id { get; set; }
        public Guid status_id { get; set; }
        public string synchronization_name { get; set; } = string.Empty;
        public string synchronization_code { get; set; } = string.Empty;
        public string synchronization_observations { get; set; } = string.Empty;
        public string synchronization_hour_to_execute { get; set; } = string.Empty;
        public IEnumerable<Guid> integrations { get; set; } = Enumerable.Empty<Guid>();
        public Guid? user_id { get; set; }
        public Guid? franchise_id { get; set; }
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public IEnumerable<SynchronizationStateResponseModel> SynchronizationStates { get; set; } = Enumerable.Empty<SynchronizationStateResponseModel>();
    }
    public class SynchronizationStateResponseModel
    {
        public Guid Id { get; set; }
        public string synchronization_status_key { get; set; } = string.Empty;
        public string synchronization_status_text { get; set; } = string.Empty;
        public string synchronization_status_color { get; set; } = string.Empty;
        public string synchronization_status_background { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;

    }
}
