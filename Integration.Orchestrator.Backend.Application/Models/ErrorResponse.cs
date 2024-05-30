namespace Integration.Orchestrator.Backend.Application.Models
{
    public class ErrorResponse
    {
        public int Code { get; set; }

        public string? Message { get; set; }

        public string? Details { get; set; }
    }
}
