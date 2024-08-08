using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Moq;
using System.Net;
using Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Connection
{
    public class ConnectionHandlerTests
    {
        private readonly Mock<IConnectionService<ConnectionEntity>> _serviceMock;
        private readonly Mock<IModuleSequenceService> _moduleSequenceServiceMock;
        private readonly ConnectionHandler _handler;

        public ConnectionHandlerTests()
        {
            _serviceMock = new Mock<IConnectionService<ConnectionEntity>>();
            _moduleSequenceServiceMock = new Mock<IModuleSequenceService>();

            _handler = new ConnectionHandler(_serviceMock.Object, _moduleSequenceServiceMock.Object);
        }

        [Fact]
        public async Task Handle_CreateConnection_Success()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(
                new ConnectionBasicInfoRequest<ConnectionCreateRequest>(
                    new ConnectionCreateRequest
                    {
                        ServerId = Guid.NewGuid(),
                        AdapterId = Guid.NewGuid(),
                        RepositoryId = Guid.NewGuid(),
                        Description = "description",
                        StatusId = Guid.NewGuid()
                    }));
            var cancellationToken = CancellationToken.None;

            _serviceMock.Setup(s => s.InsertAsync(It.IsAny<ConnectionEntity>()))
                        .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeCreated, response.Message.Messages[0]);
            Assert.NotNull(response.Message.Data);
            _serviceMock.Verify(s => s.InsertAsync(It.IsAny<ConnectionEntity>()), Times.Once);
        }

        [Fact]
        public async Task Handle_GetByCode_Success()
        {
            // Arrange
            var request = new GetByCodeConnectionCommandRequest(new ConnectionGetByCodeRequest { Code = "TestCode" });
            var cancellationToken = CancellationToken.None;
            var connectionEntity = new ConnectionEntity
            {
                id = Guid.NewGuid(),
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                description = "description",
                status_id = Guid.NewGuid()
            };

            _serviceMock.Setup(s => s.GetByCodeAsync(It.IsAny<string>()))
                        .ReturnsAsync(connectionEntity);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Application_RespondeGet, response.Message.Messages[0]);
            Assert.NotNull(response.Message.Data);
            _serviceMock.Verify(s => s.GetByCodeAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_GetAllPaginated_Success()
        {
            // Arrange
            var request = new GetAllPaginatedConnectionCommandRequest(new ConnectionGetAllPaginatedRequest());
            var cancellationToken = CancellationToken.None;
            var paginatedModel = new PaginatedModel()
            {
                Page = 1,
                Rows = 2,
                Search = "",
                SortBy = "",
                SortOrder = SortOrdering.Ascending
            };
            var connectionEntities = new List<ConnectionEntity>
            {
                new ConnectionEntity
                {
                    id = Guid.NewGuid(),
                    server_id = Guid.NewGuid(),
                    adapter_id = Guid.NewGuid(),
                    repository_id = Guid.NewGuid(),
                    description = "description1",
                    status_id = Guid.NewGuid()
                },
                new ConnectionEntity
                {
                    id = Guid.NewGuid(),
                    server_id = Guid.NewGuid(),
                    adapter_id = Guid.NewGuid(),
                    repository_id = Guid.NewGuid(),
                    description = "description2",
                    status_id = Guid.NewGuid()  }
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
    }
}
