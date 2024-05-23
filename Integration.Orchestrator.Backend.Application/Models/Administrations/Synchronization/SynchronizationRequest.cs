namespace Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization
{
    public class SynchronizationRequest
    {
        public Guid FranchiseId { get; set; }
        public string Status { get; set; }
        public string Observations { get; set; }
        public Guid UserId { get; set; }
        public DateTime HourToExecute { get; set; }
    }
}
