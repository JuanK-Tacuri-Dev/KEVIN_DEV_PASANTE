namespace Integration.Orchestrator.Backend.Application.Models.Administration.Operator
{
    public class OperatorBasicInfoRequest<T>
    {
        public T OperatorRequest { get; set; }

        public OperatorBasicInfoRequest(T connectionRequest) 
        {
            OperatorRequest = connectionRequest;
        }
     
    }
}
