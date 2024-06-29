namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueGetByIdResponse : ModelResponse<ValueGetById>
    {
    }
    public class ValueGetById : ValueRequest
    {
        public Guid Id { get; set; }
    }
}
