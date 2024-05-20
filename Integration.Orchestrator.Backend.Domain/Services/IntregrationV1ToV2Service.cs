using Integration.Orchestrator.Backend.Domain.Entities;
using Integration.Orchestrator.Backend.Domain.Ports;

namespace Integration.Orchestrator.Backend.Domain.Services
{
    public class IntregrationV1ToV2Service : IIntregrationV1ToV2Service
    {
        //private readonly IIntegrationV1Tov2Port _integrationV1Tov2Port;
        private readonly IExtractor<string> _extractor;
        private readonly ITransformator<string, string> _transformator;
        private readonly ILoader<string> _loader;

        public IntregrationV1ToV2Service(
            //IIntegrationV1Tov2Port integrationV1Tov2Port,
            IExtractor<string> extractor,
            ITransformator<string, string> transformator,
            ILoader<string> loader)
        {
            //_integrationV1Tov2Port = integrationV1Tov2Port;
            _extractor = extractor;
            _transformator = transformator;
            _loader = loader;
        }

        public async Task<bool> MigrationV1toV2()
        {
            var extractedData = await _extractor.execute();
            if (extractedData.Count() == 0)
            {
                throw new Exception("no hay elementos para extraer");
            }

            var transformedData = await _transformator.execute(extractedData);
            if (transformedData.Count() == 0)
            {
                throw new Exception("no hay elementos transformados");
            }
            await _loader.execute(transformedData);

            return true;
            //return await _integrationV1Tov2Port.MigrationV1toV2();

        }
    }
}
