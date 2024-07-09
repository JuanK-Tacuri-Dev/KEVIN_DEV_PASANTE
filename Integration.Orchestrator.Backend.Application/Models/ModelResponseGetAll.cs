namespace Integration.Orchestrator.Backend.Application.Models
{
    public class ModelResponseGetAll<T> where T : class
    {
        public int Code { get; set; }

        public string Description { get; set; }

        public T Data { get; set; }
    }
}
