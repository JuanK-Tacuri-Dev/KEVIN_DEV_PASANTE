namespace Integration.Orchestrator.Backend.Application.Models
{
    public class ModelResponse<T> where T : class
    {
        public int Code { get; set; }

        public string[] Messages { get; set; }

        public T Data { get; set; }

    }
}
