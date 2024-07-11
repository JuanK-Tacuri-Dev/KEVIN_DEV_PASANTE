namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessBasicInfoRequest<T>
    {
        public T ProcessRequest { get; set; }

        public ProcessBasicInfoRequest(T processRequest) 
        {
            ProcessRequest = processRequest;
        }
     
    }
}
