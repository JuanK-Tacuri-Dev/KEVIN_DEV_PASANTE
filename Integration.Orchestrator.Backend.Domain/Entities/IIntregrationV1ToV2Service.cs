namespace Integration.Orchestrator.Backend.Domain.Entities
{
    public interface IIntregrationV1ToV2Service
    {
        Task<bool> MigrationV1toV2();
    }
}
