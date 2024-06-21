namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class GetByCodeValueResponse : ModelResponse<GetByCodeValue>
    {
    }
    public class GetByCodeValue : ValueRequest
    {
        public Guid Id { get; set; }
    }
}
