namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Repository
{
    public class RepositoryBasicInfoRequest<T>
    {
        public T RepositoryRequest { get; set; }

        public RepositoryBasicInfoRequest(T repositoryRequest) 
        {
            RepositoryRequest = repositoryRequest;
        }
     
    }
}
