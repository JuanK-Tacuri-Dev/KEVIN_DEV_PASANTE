using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class ConnectionServiceTest
    {
        private readonly Mock<IConnectionRepository<ConnectionEntity>> _mockConnectionRepo;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly ConnectionService _service;
        public ConnectionServiceTest()
        {
            _mockConnectionRepo = new Mock<IConnectionRepository<ConnectionEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();
            _service = new ConnectionService(_mockConnectionRepo.Object,_mockCodeConfiguratorService.Object, _mockStatusService.Object);
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
            _mockConnectionRepo.Verify(repo => repo.InsertAsync(connection), Times.Once);
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
            _mockConnectionRepo.Verify(repo => repo.UpdateAsync(connection), Times.Once);
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
            _mockConnectionRepo.Verify(repo => repo.DeleteAsync(connection), Times.Once);
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
            _mockConnectionRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);
            var result = await _service.GetByIdAsync(id);

            Assert.Equal(connection, result);
            /*_mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<ConnectionEntity, bool>>>(expr =>
                expr.Compile()(connection))), Times.Once);*/
            _mockConnectionRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
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
            _mockConnectionRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()))
                .ReturnsAsync(connection);
            var result = await _service.GetByCodeAsync(code);

            Assert.Equal(connection, result);
            /*_mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<ConnectionEntity, bool>>>(expr =>
                expr.Compile()(connection))), Times.Once);*/
            _mockConnectionRepo.Verify(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
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
                Sort_order = Commons.SortOrdering.Ascending
            };
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
            var connections = new List<ConnectionEntity> { connection };
            var spec = new ConnectionSpecification(paginatedModel);
            _mockConnectionRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<ConnectionEntity>>())).ReturnsAsync(connections);

            var result = await _service.GetAllPaginatedAsync(paginatedModel);
            List<ConnectionEntity> r = result.ToList();
            Assert.Equal(connections, result);
            _mockConnectionRepo.Verify(repo => repo.GetAllAsync(It.IsAny<ConnectionSpecification>()), Times.Once);
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
                Sort_order = Commons.SortOrdering.Ascending
            };
            var totalRows = 10L;
            var spec = new ConnectionSpecification(paginatedModel);
            _mockConnectionRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<ConnectionEntity>>())).ReturnsAsync(totalRows);
            var result = await _service.GetTotalRowsAsync(paginatedModel);
            Assert.Equal(totalRows, result);
            _mockConnectionRepo.Verify(repo => repo.GetTotalRows(It.IsAny<ConnectionSpecification>()), Times.Once);
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
            _mockConnectionRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>())).ReturnsAsync(new ConnectionEntity { connection_code = code });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(connectionEntity));

            // Verificar que se lanzó la excepción y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
            Assert.Equal(code, exception.Details.Data);

            _mockStatusService.Verify(repo => repo.GetByIdAsync(statusId), Times.Once);
            _mockCodeConfiguratorService.Verify(repo => repo.GenerateCodeAsync(Prefix.Connection), Times.Once);
            _mockConnectionRepo.Verify(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<ConnectionEntity, bool>>>()), Times.Once);
        }
    }
}
