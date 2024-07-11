namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueBasicInfoRequest<T>
    {
        public T ValueRequest { get; set; }

        public ValueBasicInfoRequest(T valueRequest) 
        {
            ValueRequest = valueRequest;
        }
     
    }
}
