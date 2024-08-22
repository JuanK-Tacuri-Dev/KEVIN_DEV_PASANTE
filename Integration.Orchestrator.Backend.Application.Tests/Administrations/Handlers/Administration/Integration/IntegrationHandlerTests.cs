using Integration.Orchestrator.Backend.Application.Handlers.Administrations.Integration;
using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Moq;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Integration
{
    public class IntegrationHandlerTests
    {
        private readonly Mock<IIntegrationService<IntegrationEntity>> _serviceMock;
        private readonly IntegrationHandler _handler;

        public IntegrationHandlerTests()
        {
            _serviceMock = new Mock<IIntegrationService<IntegrationEntity>>();
            _handler = new IntegrationHandler(_serviceMock.Object);
        }

        [Fact]
        public async Task Handle_CreateIntegration_Success()
        {
            // Arrange
            var request = new CreateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationCreateRequest>(
                    new IntegrationCreateRequest
                    {
                        Name = "Test",
                        Status = Guid.NewGuid(),
                        Observations = "observation",
                        UserId = Guid.NewGuid(),
                        Process = new List<ProcessRequest>
                        {
                            new ProcessRequest
                            {
                                Id = Guid.NewGuid()
                            }
                        }
                    }));
            var cancellationToken = CancellationToken.None;

            _serviceMock.Setup(s => s.InsertAsync(It.IsAny<IntegrationEntity>()))
                        .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeCreated, response.Message.Messages[0]);
            Assert.NotNull(response.Message.Data);
            _serviceMock.Verify(s => s.InsertAsync(It.IsAny<IntegrationEntity>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateIntegrationCommandRequest_ThrowsOrchestratorException()
        {
            // Arrange
            var request = new CreateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationCreateRequest>(
                    new IntegrationCreateRequest
                    {
                        Name = "Test",
                        Status = Guid.NewGuid(),
                        Observations = "observation",
                        UserId = Guid.NewGuid(),
                        Process = new List<ProcessRequest>
                        {
                            new ProcessRequest
                            {
                                Id = Guid.NewGuid()
                            }
                        }
                    }));

            _serviceMock
                .Setup(service => service.InsertAsync(It.IsAny<IntegrationEntity>()))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Test Exception", exception.Message);
        }

        [Fact]
        public async Task Handle_UpdateIntegration_Success()
        {
            // Arrange
            var request = new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(
                    new IntegrationUpdateRequest
                    {
                        Name = "Test",
                        Status = Guid.NewGuid(),
                        Observations = "observation",
                        UserId = Guid.NewGuid(),
                        Process = new List<ProcessRequest>
                        {
                            new ProcessRequest
                            {
                                Id = Guid.NewGuid()
                            }
                        }
                    }), Guid.NewGuid());

            var integrationEntity = new IntegrationEntity
            {

            };
            var cancellationToken = CancellationToken.None;

            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<IntegrationEntity>()))
                        .Returns(Task.CompletedTask);
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(integrationEntity);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeUpdated, response.Message.Messages[0]);
            Assert.NotNull(response.Message.Data);
            _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<IntegrationEntity>()), Times.Once);
            _serviceMock.Verify(s => s.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdateIntegration_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(
                    new IntegrationUpdateRequest
                    {
                        Name = "Test",
                        Status = Guid.NewGuid(),
                        Observations = "observation",
                        UserId = Guid.NewGuid(),
                        Process = new List<ProcessRequest>
                        {
                            new ProcessRequest
                            {
                                Id = Guid.NewGuid()
                            }
                        }
                    }), Guid.NewGuid());

            var integrationEntity = new IntegrationEntity
            {

            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal(AppMessages.Application_IntegrationNotFound, exception.Message);
        }

        [Fact]
        public async Task Handle_DeleteIntegration_Success()
        {
            // Arrange
            var request = new DeleteIntegrationCommandRequest(
                new IntegrationDeleteRequest
                {
                    Id = Guid.NewGuid()
                });

            var integrationEntity = new IntegrationEntity
            {
            };
            var cancellationToken = CancellationToken.None;

            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<IntegrationEntity>()))
                        .Returns(Task.CompletedTask);
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(integrationEntity);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeDeleted, response.Message.Messages[0]);
            _serviceMock.Verify(s => s.DeleteAsync(It.IsAny<IntegrationEntity>()), Times.Once);
            _serviceMock.Verify(s => s.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DeleteIntegration_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new DeleteIntegrationCommandRequest(
                new IntegrationDeleteRequest
                {
                    Id = Guid.NewGuid()
                });

            var integrationEntity = new IntegrationEntity
            {
            };
            var cancellationToken = CancellationToken.None;

            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<IntegrationEntity>()))
                        .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal(AppMessages.Application_IntegrationNotFound, exception.Message);
        }

        [Fact]
        public async Task Handle_GetAllPaginated_Success()
        {
            // Arrange
            var paginatedModel = new IntegrationGetAllPaginatedRequest()
            {
                Page = 1,
                Rows = 2,
                Search = "",
                SortBy = "",
                SortOrder = 1
            };
            var request = new GetAllPaginatedIntegrationCommandRequest(paginatedModel);
            var cancellationToken = CancellationToken.None;

            var connectionEntities = new List<IntegrationEntity>
            {
                new IntegrationEntity
                {
                    integration_name = "Test",
                    status_id = Guid.NewGuid(),
                    integration_observations = "observation",
                    user_id = Guid.NewGuid(),
                    process = new List<Guid>
                    {
                        Guid.NewGuid()
                    }
                },
                new IntegrationEntity
                {
                    integration_name = "Test",
                    status_id = Guid.NewGuid(),
                    integration_observations = "observation",
                    user_id = Guid.NewGuid(),
                    process = new List<Guid>
                    {
                        Guid.NewGuid()
                    }
                }
            };

            _serviceMock.Setup(s => s.GetTotalRowsAsync(It.IsAny<PaginatedModel>()))
                        .ReturnsAsync(connectionEntities.Count());

            _serviceMock.Setup(s => s.GetAllPaginatedAsync(It.IsAny<PaginatedModel>()))
                        .ReturnsAsync(connectionEntities);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeGetAll, response.Message.Description);
            Assert.NotNull(response.Message.Data);
            Assert.Equal(connectionEntities.Count(), response.Message.Data.Total_rows);
            _serviceMock.Verify(s => s.GetTotalRowsAsync(It.IsAny<PaginatedModel>()), Times.Once);
            _serviceMock.Verify(s => s.GetAllPaginatedAsync(It.IsAny<PaginatedModel>()), Times.Once);
        }

        [Fact]
        public async Task Handle_GetAllPaginated_ShouldThrowArgumentException()
        {
            // Arrange
            var paginatedModel = new IntegrationGetAllPaginatedRequest()
            {
                Page = 1,
                Rows = 2,
                Search = "",
                SortBy = "",
                SortOrder = 1
            };
            var request = new GetAllPaginatedIntegrationCommandRequest(paginatedModel);
            var cancellationToken = CancellationToken.None;

            var connectionEntities = new List<IntegrationEntity>
        {
            new IntegrationEntity
            {
                integration_name = "Test",
                        status_id = Guid.NewGuid(),
                        integration_observations = "observation",
                        user_id = Guid.NewGuid(),
                        process = new List<Guid>
                        {
                            Guid.NewGuid()
                        }
            },
            new IntegrationEntity
            {
                integration_name = "Test",
                        status_id = Guid.NewGuid(),
                        integration_observations = "observation",
                        user_id = Guid.NewGuid(),
                        process = new List<Guid>
                        {
                            Guid.NewGuid()
                        }
            }
        };

            _serviceMock.Setup(s => s.GetTotalRowsAsync(It.IsAny<PaginatedModel>()))
                        .ReturnsAsync(0);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal(AppMessages.Application_IntegrationNotFound, exception.Message);
        }
    }
}
