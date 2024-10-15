namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Connection
{
    public class ConnectionBasicInfoRequest<T>
    {
        public T ConnectionRequest { get; set; }

        public ConnectionBasicInfoRequest(T connectionRequest) 
        {
            ConnectionRequest = connectionRequest;
        }
     
    }
}
