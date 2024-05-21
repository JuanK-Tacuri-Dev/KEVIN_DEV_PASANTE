using Integration.Orchestrator.Backend.Domain.Entities.V2ToV1;
using Integration.Orchestrator.Backend.Domain.Ports;

namespace Integration.Orchestrator.Backend.Domain.Services
{
    public class IntregrationV2ToV1Service : IIntregrationV2ToV1Service
    {
        //private readonly IIntegrationV2toV1Port _integrationV2toV1Port;
        private readonly IExtractor<TestEntityLegacy> _extractor;
        private readonly ITransformator<TestEntityLegacy, TestEntity> _transformator;
        private readonly ILoader<TestEntity> _loader;

        public IntregrationV2ToV1Service(
            //IIntegrationV2toV1Port integrationV2toV1Port,
            IExtractor<TestEntityLegacy> extractor,
            ITransformator<TestEntityLegacy, TestEntity> transformator,
            ILoader<TestEntity> loader)
        {
            //_integrationV2toV1Port = integrationV2toV1Port;
            _extractor = extractor;
            _transformator = transformator;
            _loader = loader;
        }

        public async Task<bool> MigrationV2toV1()
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

        }
    }
}
