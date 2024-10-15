namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    public class ServerRequest
    {
        public string Name { get; set; }
        public Guid? TypeServerId { get; set; }
        public string Url { get; set; }
        public Guid StatusId { get; set; }

    }
}
