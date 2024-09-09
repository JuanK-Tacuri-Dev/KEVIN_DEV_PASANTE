using Integration.Orchestrator.Backend.Domain.Entities.Administration;
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
        private readonly Mock<IIntegrationRepository<IntegrationEntity>> _mockRepo;
        private readonly IntegrationService _service;
        public IntegrationServiceTests()
        {
            _mockRepo = new Mock<IIntegrationRepository<IntegrationEntity>>();
            _service = new IntegrationService(_mockRepo.Object);
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

            await _service.InsertAsync(integration);

            _mockRepo.Verify(repo => repo.InsertAsync(integration), Times.Once);
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

            await _service.UpdateAsync(integration);

            _mockRepo.Verify(repo => repo.UpdateAsync(integration), Times.Once);
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

            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<IntegrationEntity, bool>>>())).ReturnsAsync(integration);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(integration, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<IntegrationEntity, bool>>>()), Times.Once);
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

            await _service.DeleteAsync(integration);

            _mockRepo.Verify(repo => repo.DeleteAsync(integration), Times.Once);
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
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<IntegrationEntity>>())).ReturnsAsync(integrations);

            var result = await _service.GetAllPaginatedAsync(paginatedModel);
            List<IntegrationEntity> r = result.ToList();
            Assert.Equal(integrations, result);
            _mockRepo.Verify(repo => repo.GetAllAsync(It.IsAny<IntegrationSpecification>()), Times.Once);
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
            _mockRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<IntegrationEntity>>())).ReturnsAsync(totalRows);

            var result = await _service.GetTotalRowsAsync(paginatedModel);

            Assert.Equal(totalRows, result);
            _mockRepo.Verify(repo => repo.GetTotalRows(It.IsAny<IntegrationSpecification>()), Times.Once);
        }
    }
}