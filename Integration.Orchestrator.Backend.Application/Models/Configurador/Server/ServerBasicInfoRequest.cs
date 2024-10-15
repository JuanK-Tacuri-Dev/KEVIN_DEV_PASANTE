namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    public class ServerBasicInfoRequest<T>
    {
        public T ServerRequest { get; set; }

        public ServerBasicInfoRequest(T serverRequest) 
        {
            ServerRequest = serverRequest;
        }
     
    }
}
