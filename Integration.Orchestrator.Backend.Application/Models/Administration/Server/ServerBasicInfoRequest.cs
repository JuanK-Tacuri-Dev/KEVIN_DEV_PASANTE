namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
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
