namespace Integration.Orchestrator.Backend.Domain.Models.Configurator
{
    public class IntegrationResponseModel
    {
        public Guid id { get; set; }
        public string integration_name { get; set; } = string.Empty;
        public string integration_observations { get; set; } = string.Empty;
        public Guid user_id { get; set; }
        public Guid status_id { get; set; }
        public IEnumerable<IntegrationProcess> process { get; set; } = Enumerable.Empty<IntegrationProcess>();
    }
    public class IntegrationProcess
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
    }
}
