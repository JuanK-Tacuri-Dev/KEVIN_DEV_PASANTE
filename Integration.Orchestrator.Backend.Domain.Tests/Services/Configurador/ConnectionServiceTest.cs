using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Services.Configurador
{
    public class ConnectionServiceTest
    {
        private readonly Mock<IConnectionRepository<ConnectionEntity>> _mockRepo;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly ConnectionService _service;
        public ConnectionServiceTest()
        {
            _mockRepo = new Mock<IConnectionRepository<ConnectionEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();
            _service = new ConnectionService(_mockRepo.Object, _mockCodeConfiguratorService.Object, _mockStatusService.Object);
        }

        [Fact]
        public async Task InsertAsync_ValidConnection_InsertsSuccessfully()
        {
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };
            _mockStatusService.Setup(repo => repo.GetByIdAsync(connection.status_id)).ReturnsAsync(new StatusEntity { });

            await _service.InsertAsync(connection);
            _mockRepo.Verify(repo => repo.InsertAsync(connection), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidConnection_UpdatesSuccessfully()
        {
            var connection = new ConnectionEntity()
            {
                connection_code = "C001",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };
            _mockStatusService.Setup(repo => repo.GetByIdAsync(connection.status_id)).ReturnsAsync(new StatusEntity { });

            await _service.UpdateAsync(connection);
            _mockRepo.Verify(repo => repo.UpdateAsync(connection), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidConnection_DeletesSuccessfully()
        {
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };
            await _service.DeleteAsync(connection);
            _mockRepo.Verify(repo => repo.DeleteAsync(connection), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsConnection()
        {
            var id = Guid.NewGuid();
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = id,
                adapter_id = id,
                repository_id = id,
                connection_description = "description",
                status_id = id
            };
            var expression = ConnectionSpecification.GetByIdExpression(id);
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);
            var result = await _service.GetByIdAsync(id);

            Assert.Equal(connection, result);
            /*_mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<ConnectionEntity, bool>>>(expr =>
                expr.Compile()(connection))), Times.Once);*/
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByCodeAsync_ValidCode_ReturnsConnection()
        {
            var code = "code";
            var connection = new ConnectionEntity()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };

            var expression = ConnectionSpecification.GetByCodeExpression(code);
            _mockRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);
            var result = await _service.GetByCodeAsync(code);

            Assert.Equal(connection, result);
            /*_mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<ConnectionEntity, bool>>>(expr =>
                expr.Compile()(connection))), Times.Once);*/
            _mockRepo.Verify(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ValidPagination_ReturnsConnections()
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
            var connection = new ConnectionResponseModel()
            {
                connection_code = "code",
                connection_name = "name",
                server_id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                connection_description = "description",
                status_id = Guid.NewGuid()
            };
            var connections = new List<ConnectionResponseModel> { connection };
            var spec = new ConnectionSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<ConnectionEntity>>())).ReturnsAsync(connections);

            var result = await _service.GetAllPaginatedAsync(paginatedModel);
            List<ConnectionResponseModel> r = result.ToList();
            Assert.Equal(connections, result);
            _mockRepo.Verify(repo => repo.GetAllAsync(It.IsAny<ConnectionSpecification>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalRowsAsync_ValidPagination_ReturnsTotalRows()
        {
            var paginatedModel = new PaginatedModel()
            {
                First = 1,
                Rows = 1,
                Search = "",
                Sort_field = "",
                Sort_order = SortOrdering.Ascending
            };
            var totalRows = 10L;
            var spec = new ConnectionSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<ConnectionEntity>>())).ReturnsAsync(totalRows);
            var result = await _service.GetTotalRowsAsync(paginatedModel);
            Assert.Equal(totalRows, result);
            _mockRepo.Verify(repo => repo.GetTotalRows(It.IsAny<ConnectionSpecification>()), Times.Once);
        }

        [Fact]
        public async Task EnsureStatusExists_ShouldThrowOrchestratorArgumentException_WhenStatusDoesNotExist()
        {
            // Arrange
            var connectionEntity = new ConnectionEntity
            {
                status_id = Guid.NewGuid() // ID de estado que no existe
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(connectionEntity));

            // Verificar que se lanzó la excepción y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
            Assert.Equal(connectionEntity.status_id, exception.Details.Data);

            _mockStatusService.Verify(repo => repo.GetByIdAsync(connectionEntity.status_id), Times.Once);
        }

        [Fact]
        public async Task EnsureCodeIsUnique_ShouldThrowOrchestratorArgumentException_WhenCodeIsNotUnique()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var code = "existing_code"; // Código que ya existe
            var connectionEntity = new ConnectionEntity
            {
                status_id = statusId
            };

            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            // Simulamos que el código ya existe
            _mockCodeConfiguratorService.Setup(repo => repo.GenerateCodeAsync(Prefix.Connection)).ReturnsAsync(code);
            _mockRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>())).ReturnsAsync(new ConnectionEntity { connection_code = code });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(connectionEntity));

            // Verificar que se lanzó la excepción y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
            Assert.Equal(code, exception.Details.Data);

            _mockStatusService.Verify(repo => repo.GetByIdAsync(statusId), Times.Once);
            _mockCodeConfiguratorService.Verify(repo => repo.GenerateCodeAsync(Prefix.Connection), Times.Once);
            _mockRepo.Verify(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalRowsAsync_ShouldReturnLong_WhenCalled()
        {
            var count = 2;
            // Arrange
            var paginatedModel = new PaginatedModel
            {
                First = 0,
                Rows = 10,
                Sort_field = "name",
                Sort_order = SortOrdering.Ascending,
                Search = "",
                activeOnly = true
            };

            _mockRepo.Setup(x => x.GetTotalRows(It.IsAny<ISpecification<ConnectionEntity>>()))
                .ReturnsAsync(count);

            // Act
            var result = await _service.GetTotalRowsAsync(paginatedModel);

            // Assert
            Assert.Equal(count, result);
            _mockRepo.Verify(x => x.GetTotalRows(It.IsAny<ISpecification<ConnectionEntity>>()), Times.Once);
        }

        [Fact]
        public async Task GetByServerIdAsync_ValidId_ReturnsConnection()
        {
            var serverId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var connection = new ConnectionEntity 
            {
                id = Guid.NewGuid(),
                adapter_id =Guid.NewGuid(),
                connection_code = "C001",
                connection_description = "Connection test",
                connection_name = "Connection test",
                server_id = serverId,
                repository_id = Guid.NewGuid(),
                status_id = statusId 
            };

            _mockRepo.Setup(repo => repo.GetByExpressionIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);

            var result = await _service.GetByServerIdAsync(serverId, statusId);

            Assert.Equal(connection, result);
            _mockRepo.Verify(repo => repo.GetByExpressionIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByAdapterIdAsync_ValidId_ReturnsConnection()
        {
            var adapterId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var connection = new ConnectionEntity
            {
                id = Guid.NewGuid(),
                adapter_id = adapterId,
                connection_code = "C001",
                connection_description = "Connection test",
                connection_name = "Connection test",
                server_id = Guid.NewGuid(),
                repository_id = Guid.NewGuid(),
                status_id = statusId
            };

            _mockRepo.Setup(repo => repo.GetByExpressionIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);

            var result = await _service.GetByAdapterIdAsync(adapterId, statusId);

            Assert.Equal(connection, result);
            _mockRepo.Verify(repo => repo.GetByExpressionIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByRepositoryIdAsync_ValidId_ReturnsConnection()
        {
            var repositoryId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var connection = new ConnectionEntity
            {
                id = Guid.NewGuid(),
                adapter_id = Guid.NewGuid(),
                connection_code = "C001",
                connection_description = "Connection test",
                connection_name = "Connection test",
                server_id = Guid.NewGuid(),
                repository_id = repositoryId,
                status_id = statusId
            };

            _mockRepo.Setup(repo => repo.GetByExpressionIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);

            var result = await _service.GetByRepositoryIdAsync(repositoryId, statusId);

            Assert.Equal(connection, result);
            _mockRepo.Verify(repo => repo.GetByExpressionIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }
        
    }
}
