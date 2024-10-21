using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryBasicInfoRequest<T>
    {
        public T RepositoryRequest { get; set; }

        public RepositoryBasicInfoRequest(T repositoryRequest) 
        {
            RepositoryRequest = repositoryRequest;
        }
     
    }
}
