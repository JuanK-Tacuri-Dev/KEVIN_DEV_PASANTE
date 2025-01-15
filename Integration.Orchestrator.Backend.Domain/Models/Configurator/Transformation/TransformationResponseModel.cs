namespace Integration.Orchestrator.Backend.Domain.Models.Configurator.Transformation
{
    public class TransformationResponseModel
    {
        public Guid id { get; set; }
        public string transformation_code { get; set; } = string.Empty;
        public string transformation_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; } 
    }
}
