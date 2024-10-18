using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Moq;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class RepositoryServiceTests
    {
        private readonly Mock<IRepositoryRepository<RepositoryEntity>> _mockRepositoryRepository;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly RepositoryService _mockRepositoryService;

        public RepositoryServiceTests()
        {
            _mockRepositoryRepository = new Mock<IRepositoryRepository<RepositoryEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();

            _mockRepositoryService = new RepositoryService(
                _mockRepositoryRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );
        }
    }
}
