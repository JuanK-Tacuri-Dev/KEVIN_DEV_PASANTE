using Integration.Orchestrator.Backend.Domain.Entities;
using Integration.Orchestrator.Backend.Domain.Ports;

namespace Integration.Orchestrator.Backend.Domain.Services
{
    public class IntregrationV1ToV2Service: IIntregrationV1ToV2Service
    {
        private readonly IIntegrationV1Tov2Port _integrationV1Tov2Port;

        public IntregrationV1ToV2Service(IIntegrationV1Tov2Port integrationV1Tov2Port) 
        {
            _integrationV1Tov2Port = integrationV1Tov2Port;
        }

        public async Task<bool> MigrationV1toV2()
        {
            return await _integrationV1Tov2Port.MigrationV1toV2();
        }
    }
}
