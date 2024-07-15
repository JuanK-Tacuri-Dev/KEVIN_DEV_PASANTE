namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
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
