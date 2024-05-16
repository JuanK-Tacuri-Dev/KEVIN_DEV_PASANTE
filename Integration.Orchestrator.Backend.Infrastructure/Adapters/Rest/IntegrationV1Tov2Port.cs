using Integration.Orchestrator.Backend.Domain.Ports;

namespace Integration.Orchestrator.Backend.Infrastructure.DataAccess.Rest
{
    public class IntegrationV1Tov2Port : IIntegrationV1Tov2Port
    {
        public IntegrationV1Tov2Port() 
        { 
        }
        public async Task<bool> MigrationV1toV2()
        {
            //consumo rest
            return await Task.Run(() => true);
        }
    }
}
