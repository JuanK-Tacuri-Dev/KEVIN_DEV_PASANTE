namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
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
