namespace Integration.Orchestrator.Backend.Domain.Exceptions
{
    public class OrchestratorArgumentException(string message , DetailsArgumentErrors details)
        : ArgumentException(message)
    {
        private DetailsArgumentErrors DetailsError { get; } = details;

        public DetailsArgumentErrors Details => DetailsError;
    }

    public class DetailsArgumentErrors
    {
        public DetailsArgumentErrors()
        {
                
        }
        public DetailsArgumentErrors(int Code,string Description,object Data)
        {
            this.Code = Code;
            this.Description = Description;
            this.Data = Data;

        }
        public int Code { get; set; }
        public string Description { get; set; } = string.Empty;
        public object Data { get; set; } = string.Empty;
    }


}

