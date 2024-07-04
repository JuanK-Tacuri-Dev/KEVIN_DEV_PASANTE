namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates
{
    public class SynchronizationStatesGetByIdResponse : ModelResponse<SynchronizationStatesGetById>
    {
    }
    public class SynchronizationStatesGetById : SynchronizationStatesRequest
    {
        public Guid Id { get; set; }
    }
}
