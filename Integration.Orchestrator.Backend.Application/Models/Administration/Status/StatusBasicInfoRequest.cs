namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
{
    public class StatusBasicInfoRequest<T>
    {
        public T StatusRequest { get; set; }

        public StatusBasicInfoRequest(T statusRequest) 
        {
            StatusRequest = statusRequest;
        }
     
    }
}
