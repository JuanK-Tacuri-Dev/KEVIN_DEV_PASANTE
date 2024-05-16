namespace Integration.Orchestrator.Backend.Domain.Ports
{
    public interface IIntegrationV1Tov2Port
    {
        Task<bool> MigrationV1toV2();
    }
}
