namespace Integration.Orchestrator.Backend.Application.Models
{
    public class ModelResponseGetAll<T> : ModelResponse<T> where T : class
    {
        public int? TotalRows { get; set; }
    }
}
