using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
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
