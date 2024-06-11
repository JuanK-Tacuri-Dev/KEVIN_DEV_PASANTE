using Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Moq;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers
{
    public class SynchronizationHandlerTests
    {
        private readonly Mock<ISynchronizationService<SynchronizationEntity>> _mockService;
        private readonly SynchronizationHandler _handler;

        public SynchronizationHandlerTests()
        {
            _mockService = new Mock<ISynchronizationService<SynchronizationEntity>>();
            _handler = new SynchronizationHandler(_mockService.Object);
        }

        [Fact]
        public async Task Handle_CreateSynchronizationCommandRequest_ShouldReturnCorrectResponse()
        {
            var synchronizationCreateRequest = new SynchronizationCreateRequest
            {
                Name = "Test Name",
                FranchiseId = Guid.NewGuid(),
                Status = Guid.NewGuid(),
                Observations = "Test Observations",
                HourToExecute = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                Integrations = new List<IntegrationRequest> { new IntegrationRequest { Id = Guid.NewGuid() } },
                UserId = Guid.NewGuid()
            };
            // Arrange
            var request = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(synchronizationCreateRequest));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockService.Verify(service => service.InsertAsync(It.IsAny<SynchronizationEntity>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK.GetHashCode(), response.Message.Code);
            Assert.Equal(AppMessages.Application_SynchronizationResponseCreated, response.Message.Description);
        }

        [Fact]
        public async Task Handle_UpdateSynchronizationCommandRequest_ShouldReturnCorrectResponse()
        {
            // Arrange
            var synchronizationId = Guid.NewGuid();
            var integrationId = Guid.NewGuid();
            var name = "name";
            var franchiseId = Guid.NewGuid();
            var status = Guid.NewGuid();
            var observation = "Test Observations";
            var hour_to_execute = DateTime.Now;
            var user_id = Guid.NewGuid();

            var existingEntity = new SynchronizationEntity
            {
                id = synchronizationId,
                name = name,
                franchise_id = franchiseId,
                status = status,
                observations = observation,
                hour_to_execute = hour_to_execute,
                integrations = new List<Guid> { integrationId },
                user_id = user_id
            };
            _mockService.Setup(service => service.GetByIdAsync(synchronizationId)).ReturnsAsync(existingEntity);

            var synchronizationUpdateRequest = new SynchronizationUpdateRequest
            {
                Name = name,
                FranchiseId = franchiseId,
                Status = status,
                Observations = observation,
                HourToExecute = hour_to_execute.ToString(),
                Integrations = new List<IntegrationRequest> { new IntegrationRequest { Id = integrationId } },
                UserId = user_id
            };
            // Arrange
            var request = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(synchronizationUpdateRequest), synchronizationId);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockService.Verify(service => service.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _mockService.Verify(service => service.UpdateAsync(It.IsAny<SynchronizationEntity>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK.GetHashCode(), response.Message.Code);
            Assert.Equal(AppMessages.Application_SynchronizationResponseUpdated, response.Message.Description);
        }

        [Fact]
        public async Task Handle_UpdateSynchronizationCommandRequest_ShouldReturnIncorrectResponse()
        {
            // Arrange
            var synchronizationId = Guid.NewGuid();
            var integrationId = Guid.NewGuid();
            var name = "name";
            var franchiseId = Guid.NewGuid();
            var status = Guid.NewGuid();
            var observation = "Test Observations";
            var hour_to_execute = DateTime.Now;
            var user_id = Guid.NewGuid();

            var synchronizationUpdateRequest = new SynchronizationUpdateRequest
            {
                Name = name,
                FranchiseId = franchiseId,
                Status = status,
                Observations = observation,
                HourToExecute = hour_to_execute.ToString(),
                Integrations = new List<IntegrationRequest> { new IntegrationRequest { Id = integrationId } },
                UserId = user_id
            };
            // Arrange
            var request = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(synchronizationUpdateRequest), synchronizationId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal(AppMessages.Application_SynchronizationNotFound, exception.Message);
        }

        [Fact]
        public async Task Handle_DeleteSynchronizationCommandRequest_ShouldReturnCorrectResponse()
        {
            // Arrange
            var synchronizationId = Guid.NewGuid();
            var integrationId = Guid.NewGuid();
            var name = "name";
            var franchiseId = Guid.NewGuid();
            var status = Guid.NewGuid();
            var observation = "Test Observations";
            var hour_to_execute = DateTime.Now;
            var user_id = Guid.NewGuid();
            var existingEntity = new SynchronizationEntity
            {
                id = synchronizationId,
                name = name,
                franchise_id = franchiseId,
                status = status,
                observations = observation,
                hour_to_execute = hour_to_execute,
                user_id = user_id
            };
            _mockService.Setup(service => service.GetByIdAsync(synchronizationId)).ReturnsAsync(existingEntity);

            var request = new DeleteSynchronizationCommandRequest
            {
                Synchronization = new SynchronizationDeleteRequest { Id = synchronizationId }
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockService.Verify(service => service.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _mockService.Verify(service => service.DeleteAsync(It.IsAny<SynchronizationEntity>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK.GetHashCode(), response.Message.Code);
            Assert.Equal(AppMessages.Application_SynchronizationResponseDeleted, response.Message.Description);
        }

        [Fact]
        public async Task Handle_DeleteSynchronizationCommandRequest_ShouldReturnIncorrectResponse()
        {
            // Arrange
            var synchronizationId = Guid.NewGuid();
            var integrationId = Guid.NewGuid();
            var name = "name";
            var franchiseId = Guid.NewGuid();
            var status = Guid.NewGuid();
            var observation = "Test Observations";
            var hour_to_execute = DateTime.Now;
            var user_id = Guid.NewGuid();
            var existingEntity = new SynchronizationEntity
            {
                id = synchronizationId,
                name = name,
                franchise_id = franchiseId,
                status = status,
                observations = observation,
                hour_to_execute = hour_to_execute,
                user_id = user_id
            };

            var request = new DeleteSynchronizationCommandRequest
            {
                Synchronization = new SynchronizationDeleteRequest { Id = synchronizationId }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal(AppMessages.Application_SynchronizationNotFound, exception.Message);
        }

        [Fact]
        public async Task Handle_GetByFranchiseIdSynchronizationCommandRequest_ShouldReturnCorrectResponse()
        {
            // Arrange
            var franchiseId = Guid.NewGuid();
            var synchronizations = new List<SynchronizationEntity>
        {
            new SynchronizationEntity { id = Guid.NewGuid(), franchise_id = franchiseId, name = "Test Name", integrations = new List<Guid> { Guid.NewGuid() } }
        };
            _mockService.Setup(service => service.GetByFranchiseIdAsync(franchiseId)).ReturnsAsync(synchronizations);

            var request = new GetByFranchiseIdSynchronizationCommandRequest
            {
                Synchronization = new GetByFranchiseIdSynchronizationRequest { FranchiseId = franchiseId }
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockService.Verify(service => service.GetByFranchiseIdAsync(franchiseId), Times.Once);
            Assert.Equal(HttpStatusCode.OK.GetHashCode(), response.Message.Code);
            Assert.Equal(AppMessages.Api_SynchronizationResponse, response.Message.Description);
            Assert.Equal(synchronizations.Count, response.Message.Data.Count());
        }

        [Fact]
        public async Task Handle_GetByFranchiseIdSynchronizationCommandRequest_ShouldReturnIncorrectResponse()
        {
            // Arrange
            var franchiseId = Guid.NewGuid();
            var synchronizations = new List<SynchronizationEntity>
        {
            new SynchronizationEntity { id = Guid.NewGuid(), franchise_id = franchiseId, name = "Test Name", integrations = new List<Guid> { Guid.NewGuid() } }
        };

            var request = new GetByFranchiseIdSynchronizationCommandRequest
            {
                Synchronization = new GetByFranchiseIdSynchronizationRequest { FranchiseId = franchiseId }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal(AppMessages.Application_SynchronizationNotFound, exception.Message);
        }

        [Fact]
        public async Task Handle_GetAllPaginatedSynchronizationCommandRequest_ShouldReturnCorrectResponse()
        {
            // Arrange
            var paginatedModel = new PaginatedModel { Page = 1, Rows = 10, SortBy = "" };
            var synchronizations = new List<SynchronizationEntity>
        {
            new SynchronizationEntity { id = Guid.NewGuid(), name = "Test Name", integrations = new List<Guid> { Guid.NewGuid()} }
        };
            _mockService.Setup(service => service.GetTotalRowsAsync(It.IsAny<PaginatedModel>())).ReturnsAsync(1);
            _mockService.Setup(service => service.GetAllPaginatedAsync(It.IsAny<PaginatedModel>())).ReturnsAsync(synchronizations);

            var request = new GetAllPaginatedSynchronizationCommandRequest
            {
                Synchronization = new SynchronizationGetAllPaginatedRequest { Page = 1, Rows = 10, SortBy = "" }
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockService.Verify(service => service.GetTotalRowsAsync(It.IsAny<PaginatedModel>()), Times.Once);
            _mockService.Verify(service => service.GetAllPaginatedAsync(It.IsAny<PaginatedModel>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK.GetHashCode(), response.Message.Code);
            Assert.Equal(AppMessages.Api_SynchronizationResponse, response.Message.Description);
            Assert.Equal(synchronizations.Count, response.Message.Data.Count());
        }

        [Fact]
        public async Task Handle_GetAllPaginatedSynchronizationCommandRequest_ShouldReturnIncorrectResponse()
        {
            // Arrange
            var paginatedModel = new PaginatedModel { Page = 1, Rows = 10, SortBy = "" };
            var synchronizations = new List<SynchronizationEntity>
        {
            new SynchronizationEntity { id = Guid.NewGuid(), name = "Test Name", integrations = new List<Guid> { Guid.NewGuid()} }
        };
            var request = new GetAllPaginatedSynchronizationCommandRequest
            {
                Synchronization = new SynchronizationGetAllPaginatedRequest { Page = 1, Rows = 10, SortBy = "" }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal(AppMessages.Application_SynchronizationNotFound, exception.Message);
        }
    }
}
