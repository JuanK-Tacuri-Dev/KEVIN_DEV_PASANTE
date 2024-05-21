namespace Integration.Orchestrator.Backend.Domain.Entities.V2ToV1
{
    public interface IIntregrationV2ToV1Service
    {
        Task<bool> MigrationV2toV1();
    }
}
