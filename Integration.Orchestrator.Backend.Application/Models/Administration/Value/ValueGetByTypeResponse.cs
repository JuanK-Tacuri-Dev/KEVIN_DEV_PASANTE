namespace Integration.Orchestrator.Backend.Application.Models.Administration.Value
{
    public class ValueGetByTypeResponse : ModelResponse<IEnumerable<ValueGetByType>>
    {
    }
    public class ValueGetByType : ValueRequest
    {
        public Guid Id { get; set; }
    }
}
