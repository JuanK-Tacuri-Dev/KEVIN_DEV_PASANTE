using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurator;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Configurator;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Services.Configurator
{
    public class IntegrationServiceTests
    {
        private readonly Mock<IIntegrationRepository<IntegrationEntity>> _mockIntegrationRepo;
        private readonly IntegrationService _mockIntegrationService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        public IntegrationServiceTests()
        {
            _mockIntegrationRepo = new Mock<IIntegrationRepository<IntegrationEntity>>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();
            _mockIntegrationService = new IntegrationService(_mockIntegrationRepo.Object, _mockStatusService.Object);
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
            _mockStatusService.Setup(repo => repo.GetByIdAsync(integration.status_id)).ReturnsAsync(new StatusEntity { });


            await _mockIntegrationService.InsertAsync(integration);

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
            _mockStatusService.Setup(repo => repo.GetByIdAsync(integration.status_id)).ReturnsAsync(new StatusEntity { });


            await _mockIntegrationService.UpdateAsync(integration);

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

            var result = await _mockIntegrationService.GetByIdAsync(id);

            Assert.Equal(integration, result);
            _mockIntegrationRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<IntegrationEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByProcessIdAsync_ShouldReturnEntityFromRepository()
        {
            var processId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var integration = new IntegrationEntity
            {
                id = Guid.NewGuid(),
                integration_name = "Integration",
                status_id = Guid.NewGuid(),
                integration_observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<Guid>
                {
                    processId,
                    Guid.NewGuid()
                }
            };

            _mockIntegrationRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<IntegrationEntity, bool>>>()))
                .ReturnsAsync(integration);

            var result = await _mockIntegrationService.GetByProcessIdAsync(processId, statusId);

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

            await _mockIntegrationService.DeleteAsync(integration);

            _mockIntegrationRepo.Verify(repo => repo.DeleteAsync(integration), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnEntitiesFromRepository()
        {
            var paginatedModel = new PaginatedModel()
            {
                First = 1,
                Rows = 1,
                Search = "Integration",
                Sort_field = "",
                Sort_order = SortOrdering.Ascending,
                activeOnly = true
            };

            var integration = new IntegrationResponseModel
            {
                id = Guid.NewGuid(),
                integration_name = "Integration",
                status_id = Guid.NewGuid(),
                integration_observations = "Observation",
                user_id = Guid.NewGuid(),
                process = new List<IntegrationProcess>
                {
                    new IntegrationProcess
                    {
                        id = Guid.NewGuid(),
                        name = "Test"
                    }
                }
            };
            var integrations = new List<IntegrationResponseModel> { integration };
            var spec = new IntegrationSpecification(paginatedModel);
            _mockIntegrationRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<IntegrationEntity>>())).ReturnsAsync(integrations);

            var result = await _mockIntegrationService.GetAllPaginatedAsync(paginatedModel);
            List<IntegrationResponseModel> r = result.ToList();
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
                Sort_order = SortOrdering.Ascending,
                activeOnly = true
            };
            var totalRows = 10L;
            var spec = new IntegrationSpecification(paginatedModel);
            _mockIntegrationRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<IntegrationEntity>>())).ReturnsAsync(totalRows);

            var result = await _mockIntegrationService.GetTotalRowsAsync(paginatedModel);

            Assert.Equal(totalRows, result);
            _mockIntegrationRepo.Verify(repo => repo.GetTotalRows(It.IsAny<IntegrationSpecification>()), Times.Once);
        }

        [Fact]
        public async Task ValidateProcessMinTwo_ShouldThrowArgumentException_WhenLessThanTwoProcesses()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var integration = new IntegrationEntity
            {
                status_id = statusId,
                process = new List<Guid> { Guid.NewGuid() } // Solo un proceso
            };
            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockIntegrationService.InsertAsync(integration));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_IntegrationMinTwoRequired, exception.Details.Description);
            Assert.Equal(integration, exception.Details.Data);
        }

        [Fact]
        public async Task EnsureStatusExists_ShouldThrowOrchestratorArgumentException_WhenStatusDoesNotExist()
        {
            // Arrange
            var integration = new IntegrationEntity
            {
                status_id = Guid.NewGuid() // ID de estado que no existe
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockIntegrationService.InsertAsync(integration));

            // Verificar que se lanz� la excepci�n y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
            Assert.Equal(integration.status_id, exception.Details.Data);
            _mockStatusService.Verify(repo => repo.GetByIdAsync(integration.status_id), Times.Once);
        }
    }
}