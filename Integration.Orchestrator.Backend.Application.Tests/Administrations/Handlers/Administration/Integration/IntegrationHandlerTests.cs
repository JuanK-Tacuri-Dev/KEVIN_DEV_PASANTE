using Integration.Orchestrator.Backend.Application.Handlers.Administrations.Integration;
using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
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
            Assert.Equal(AppMessages.Application_IntegrationResponseCreated, response.Message.Description);
            Assert.NotNull(response.Message.Data);
            _serviceMock.Verify(s => s.InsertAsync(It.IsAny<IntegrationEntity>()), Times.Once);
        }

        [Fact]
        public async Task Handle_GetAllPaginated_Success()
        {
            // Arrange
            var request = new GetAllPaginatedIntegrationCommandRequest(new IntegrationGetAllPaginatedRequest());
            var cancellationToken = CancellationToken.None;
            var paginatedModel = new PaginatedModel() 
            { 
                Page = 1,
                Rows = 2,
                Search = "",
                SortBy = "",
                SortOrder = SortOrdering.Ascending
            };
            var connectionEntities = new List<IntegrationEntity>
        {
            new IntegrationEntity 
            {
                name = "Test",
                        status = Guid.NewGuid(),
                        observations = "observation",
                        user_id = Guid.NewGuid(),
                        process = new List<Guid>
                        {
                            Guid.NewGuid()
                        }
            },
            new IntegrationEntity 
            {
                name = "Test",
                        status = Guid.NewGuid(),
                        observations = "observation",
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
            Assert.Equal(AppMessages.Api_IntegrationResponse, response.Message.Description);
            Assert.NotNull(response.Message.Data);
            Assert.Equal(connectionEntities.Count(), response.Message.Data.Count());
            Assert.Equal(connectionEntities.Count(), response.Message.TotalRows);
            _serviceMock.Verify(s => s.GetTotalRowsAsync(It.IsAny<PaginatedModel>()), Times.Once);
            _serviceMock.Verify(s => s.GetAllPaginatedAsync(It.IsAny<PaginatedModel>()), Times.Once);
        }
    }
}
