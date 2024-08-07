namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueGetByCodeResponse : ModelResponse<ValueGetByCode>
    {
    }
    public class ValueGetByCode : ValueRequest
    {
        public Guid Id { get; set; }
    }
}
