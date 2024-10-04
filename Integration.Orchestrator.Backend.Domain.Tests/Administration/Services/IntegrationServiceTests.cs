using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class IntegrationServiceTests
    {
        private readonly Mock<IIntegrationRepository<IntegrationEntity>> _mockIntegrationRepo;
        private readonly IntegrationService _integrationService;
        private readonly Mock<IStatusService<StatusEntity>> _statusService;
        public IntegrationServiceTests()
        {
            _mockIntegrationRepo = new Mock<IIntegrationRepository<IntegrationEntity>>();
            _statusService = new Mock<IStatusService<StatusEntity>>();
            _integrationService = new IntegrationService(_mockIntegrationRepo.Object, _statusService.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldCallInsertOnRepository()
        {
            var integration = new IntegrationEntity
            {
                id = Guid.NewGuid(),
                integration_name = "Integration",
                status_id = Guid.NewGuid(),
                integration_observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<Guid> 
                { 
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            };

            await _integrationService.InsertAsync(integration);

            _mockIntegrationRepo.Verify(repo => repo.InsertAsync(integration), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdateOnRepository()
        {
            var integration = new IntegrationEntity
            {
                id = Guid.NewGuid(),
                integration_name = "Integration",
                status_id = Guid.NewGuid(),
                integration_observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<Guid>
                { 
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            };

            await _integrationService.UpdateAsync(integration);

            _mockIntegrationRepo.Verify(repo => repo.UpdateAsync(integration), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntityFromRepository()
        {
            var id = Guid.NewGuid();
            var integration = new IntegrationEntity
            {
                id = Guid.NewGuid(),
                integration_name = "Integration",
                status_id = Guid.NewGuid(),
                integration_observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<Guid>
                { 
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            };

            var expression = IntegrationSpecification.GetByIdExpression(id);

            _mockIntegrationRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<IntegrationEntity, bool>>>())).ReturnsAsync(integration);

            var result = await _integrationService.GetByIdAsync(id);

            Assert.Equal(integration, result);
            _mockIntegrationRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<IntegrationEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOnRepository()
        {
            var integration = new IntegrationEntity
            {
                id = Guid.NewGuid(),
                integration_name = "Integration",
                status_id = Guid.NewGuid(),
                integration_observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<Guid>
                { 
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            };

            await _integrationService.DeleteAsync(integration);

            _mockIntegrationRepo.Verify(repo => repo.DeleteAsync(integration), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnEntitiesFromRepository()
        {
            var paginatedModel = new PaginatedModel()
            {
                First = 1,
                Rows = 1,
                Search = "",
                Sort_field = "",
                Sort_order = Commons.SortOrdering.Ascending
            };

            var integration = new IntegrationEntity
            {
                id = Guid.NewGuid(),
                integration_name = "Integration",
                status_id = Guid.NewGuid(),
                integration_observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<Guid>
                { 
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            };
            var integrations = new List<IntegrationEntity> { integration };
            var spec = new IntegrationSpecification(paginatedModel);
            _mockIntegrationRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<IntegrationEntity>>())).ReturnsAsync(integrations);

            var result = await _integrationService.GetAllPaginatedAsync(paginatedModel);
            List<IntegrationEntity> r = result.ToList();
            Assert.Equal(integrations, result);
            _mockIntegrationRepo.Verify(repo => repo.GetAllAsync(It.IsAny<IntegrationSpecification>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalRowsAsync_ShouldReturnTotalRowsFromRepository()
        {
            var paginatedModel = new PaginatedModel()
            {
                First = 1,
                Rows = 1,
                Search = "",
                Sort_field = "",
                Sort_order = Commons.SortOrdering.Ascending
            };
            var totalRows = 10L;
            var spec = new IntegrationSpecification(paginatedModel);
            _mockIntegrationRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<IntegrationEntity>>())).ReturnsAsync(totalRows);

            var result = await _integrationService.GetTotalRowsAsync(paginatedModel);

            Assert.Equal(totalRows, result);
            _mockIntegrationRepo.Verify(repo => repo.GetTotalRows(It.IsAny<IntegrationSpecification>()), Times.Once);
        }
    }
}