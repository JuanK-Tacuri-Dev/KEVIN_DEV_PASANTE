using Integration.Orchestrator.Backend.Application.Handlers.Administrations.Connection;
using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Moq;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Connection
{
    public class ConnectionHandlerTests
    {
        private readonly Mock<IConnectionService<ConnectionEntity>> _serviceMock;
        private readonly ConnectionHandler _handler;

        public ConnectionHandlerTests()
        {
            _serviceMock = new Mock<IConnectionService<ConnectionEntity>>();
            _handler = new ConnectionHandler(_serviceMock.Object);
        }

        [Fact]
        public async Task Handle_CreateConnection_Success()
        {
            // Arrange
            var request = new CreateConnectionCommandRequest(
                new ConnectionBasicInfoRequest<ConnectionCreateRequest>(
                    new ConnectionCreateRequest
                    {
                        Code = "TestCode", 
                        Server = "TestServer", 
                        Port = "1234", 
                        User = "TestUser", 
                        Password = "TestPassword", 
                        Adapter = "TestAdapter" }));
            var cancellationToken = CancellationToken.None;

            _serviceMock.Setup(s => s.InsertAsync(It.IsAny<ConnectionEntity>()))
                        .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Application_ConnectionResponseCreated, response.Message.Description);
            Assert.NotNull(response.Message.Data);
            _serviceMock.Verify(s => s.InsertAsync(It.IsAny<ConnectionEntity>()), Times.Once);
        }

        [Fact]
        public async Task Handle_GetByCode_Success()
        {
            // Arrange
            var request = new GetByCodeConnectionCommandRequest(new GetByCodeConnectionRequest { Code = "TestCode" });
            var cancellationToken = CancellationToken.None;
            var connectionEntity = new ConnectionEntity 
            { 
                id = Guid.NewGuid(), 
                connection_code = "TestCode", 
                server = "TestServer", 
                port = "1234", 
                user = "TestUser", 
                password = "TestPassword", 
                adapter = "TestAdapter" };

            _serviceMock.Setup(s => s.GetByCodeAsync(It.IsAny<string>()))
                        .ReturnsAsync(connectionEntity);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Api_ConnectionResponse, response.Message.Description);
            Assert.NotNull(response.Message.Data);
            _serviceMock.Verify(s => s.GetByCodeAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_GetByType_Success()
        {
            // Arrange
            var request = new GetByTypeConnectionCommandRequest(new GetByTypeConnectionRequest { Type = "TestType" });
            var cancellationToken = CancellationToken.None;
            var connectionEntities = new List<ConnectionEntity>
        {
            new ConnectionEntity { id = Guid.NewGuid(), connection_code = "TestCode1", server = "TestServer1", port = "1234", user = "TestUser1", password = "TestPassword1", adapter = "TestAdapter1" },
            new ConnectionEntity { id = Guid.NewGuid(), connection_code = "TestCode2", server = "TestServer2", port = "1234", user = "TestUser2", password = "TestPassword2", adapter = "TestAdapter2" }
        };

            _serviceMock.Setup(s => s.GetByTypeAsync(It.IsAny<string>()))
                        .ReturnsAsync(connectionEntities);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Api_ConnectionResponse, response.Message.Description);
            Assert.NotNull(response.Message.Data);
            Assert.Equal(connectionEntities.Count(), response.Message.Data.Count());
            _serviceMock.Verify(s => s.GetByTypeAsync(It.IsAny<string>()), Times.Once);
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
            new ConnectionEntity { id = Guid.NewGuid(), connection_code = "TestCode1", server = "TestServer1", port = "1234", user = "TestUser1", password = "TestPassword1", adapter = "TestAdapter1" },
            new ConnectionEntity { id = Guid.NewGuid(), connection_code = "TestCode2", server = "TestServer2", port = "1234", user = "TestUser2", password = "TestPassword2", adapter = "TestAdapter2" }
        };

            _serviceMock.Setup(s => s.GetTotalRowsAsync(It.IsAny<PaginatedModel>()))
                        .ReturnsAsync(connectionEntities.Count());

            _serviceMock.Setup(s => s.GetAllPaginatedAsync(It.IsAny<PaginatedModel>()))
                        .ReturnsAsync(connectionEntities);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.Message.Code);
            Assert.Equal(AppMessages.Api_ConnectionResponse, response.Message.Description);
            Assert.NotNull(response.Message.Data);
            Assert.Equal(connectionEntities.Count(), response.Message.Data.Count());
            Assert.Equal(connectionEntities.Count(), response.Message.TotalRows);
            _serviceMock.Verify(s => s.GetTotalRowsAsync(It.IsAny<PaginatedModel>()), Times.Once);
            _serviceMock.Verify(s => s.GetAllPaginatedAsync(It.IsAny<PaginatedModel>()), Times.Once);
        }
    }
}
