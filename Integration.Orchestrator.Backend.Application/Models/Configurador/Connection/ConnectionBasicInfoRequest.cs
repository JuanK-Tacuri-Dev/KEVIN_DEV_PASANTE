using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionBasicInfoRequest<T>
    {
        public T ConnectionRequest { get; set; }

        public ConnectionBasicInfoRequest(T connectionRequest) 
        {
            ConnectionRequest = connectionRequest;
        }
     
    }
}
