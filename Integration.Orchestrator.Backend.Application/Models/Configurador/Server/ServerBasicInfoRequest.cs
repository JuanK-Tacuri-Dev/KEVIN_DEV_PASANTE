using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerBasicInfoRequest<T>
    {
        public T ServerRequest { get; set; }

        public ServerBasicInfoRequest(T serverRequest) 
        {
            ServerRequest = serverRequest;
        }
     
    }
}
