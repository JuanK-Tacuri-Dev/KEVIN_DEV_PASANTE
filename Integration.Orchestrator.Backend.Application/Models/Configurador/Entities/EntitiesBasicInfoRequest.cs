namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Entities
{
    public class EntitiesBasicInfoRequest<T>
    {
        public T EntitiesRequest { get; set; }

        public EntitiesBasicInfoRequest(T entitiesRequest) 
        {
            EntitiesRequest = entitiesRequest;
        }
     
    }
}
