namespace Integration.Orchestrator.Backend.Domain.Models.Configurator
{
    public class IntegrationResponseModel
    {
        public Guid id { get; set; }
        public string integration_name { get; set; }
        public string integration_observations { get; set; }
        public Guid user_id { get; set; }
        public Guid status_id { get; set; }
        public List<IntegrationProcess> process { get; set; }
    }
    public class IntegrationProcess
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}
