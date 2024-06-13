namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
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
