using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Configurador;
using Moq;
using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Tests.Services.Configurador
{
    public class ServerServiceTests
    {
        private readonly Mock<IServerRepository<ServerEntity>> _mockServerRepository;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly ServerService _mockServerService;

        public ServerServiceTests()
        {
            _mockServerRepository = new Mock<IServerRepository<ServerEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();

            _mockServerService = new ServerService(
                _mockServerRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );
        }

        [Fact]
        public async Task InsertAsync_Should_Insert_Server_Successfully()
        {
            // Arrange
            var statusId = Guid.NewGuid();
            var server = new ServerEntity { id = Guid.NewGuid(), status_id = statusId };
            var generatedCode = "SVR-001";

            _mockStatusService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new StatusEntity { id = statusId });

            _mockCodeConfiguratorService.Setup(x => x.GenerateCodeAsync(It.IsAny<Prefix>()))
                .ReturnsAsync(generatedCode);

            _mockServerRepository.Setup(x => x.ValidateNameURL(It.IsAny<ServerEntity>()))
                .ReturnsAsync(false);

            _mockServerRepository.Setup(x => x.InsertAsync(It.IsAny<ServerEntity>()));

            // Act
            await _mockServerService.InsertAsync(server);

            // Assert
            _mockStatusService.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _mockCodeConfiguratorService.Verify(x => x.GenerateCodeAsync(It.IsAny<Prefix>()), Times.Once);
            _mockServerRepository.Verify(x => x.InsertAsync(It.IsAny<ServerEntity>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Throw_Exception_When_Status_Not_Found()
        {
            // Arrange
            var server = new ServerEntity { id = Guid.NewGuid(), status_id = Guid.NewGuid() };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockServerService.InsertAsync(server));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
            Assert.Equal(server.status_id, exception.Details.Data);
        }

        [Fact]
        public async Task InsertAsync_Should_Throw_Exception_When_Duplicate_NameUrl()
        {
            // Arrange
            var statusId = Guid.NewGuid();
            var server = new ServerEntity { id = Guid.NewGuid(), status_id = statusId };

            _mockStatusService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new StatusEntity { id = statusId });

            _mockServerRepository.Setup(x => x.ValidateNameURL(It.IsAny<ServerEntity>()))
                .ReturnsAsync(true);  // Simulamos duplicidad de nombre y URL

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockServerService.InsertAsync(server));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_ServerExists, exception.Details.Description);
            Assert.Equal(server, exception.Details.Data);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Server_Successfully()
        {
            // Arrange
            var statusId = Guid.NewGuid();
            var server = new ServerEntity { id = Guid.NewGuid(), status_id = Guid.NewGuid() };

            _mockStatusService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new StatusEntity { id = statusId });

            _mockServerRepository.Setup(x => x.ValidateNameURL(It.IsAny<ServerEntity>()))
                .ReturnsAsync(false);

            _mockServerRepository.Setup(x => x.UpdateAsync(It.IsAny<ServerEntity>()));

            // Act
            await _mockServerService.UpdateAsync(server);

            // Assert
            _mockStatusService.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _mockServerRepository.Verify(x => x.UpdateAsync(It.IsAny<ServerEntity>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Server_Successfully()
        {
            // Arrange
            var server = new ServerEntity { id = Guid.NewGuid() };

            _mockServerRepository.Setup(x => x.DeleteAsync(It.IsAny<ServerEntity>()));

            // Act
            await _mockServerService.DeleteAsync(server);

            // Assert
            _mockServerRepository.Verify(x => x.DeleteAsync(It.IsAny<ServerEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Server_By_Id()
        {
            // Arrange
            var statusId = Guid.NewGuid();
            var serverId = Guid.NewGuid();
            var server = new ServerEntity { id = serverId, status_id = statusId };

            _mockServerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<ServerEntity, bool>>>()))
                .ReturnsAsync(server);

            // Act
            var result = await _mockServerService.GetByIdAsync(serverId);

            // Assert
            Assert.Equal(serverId, result.id);
            _mockServerRepository.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<ServerEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByCodeAsync_Should_Return_Server_By_Code()
        {
            // Arrange
            var serverCode = "SVR-001";
            var server = new ServerEntity { server_code = serverCode };

            _mockServerRepository.Setup(x => x.GetByCodeAsync(It.IsAny<Expression<Func<ServerEntity, bool>>>()))
                .ReturnsAsync(server);

            // Act
            var result = await _mockServerService.GetByCodeAsync(serverCode);

            // Assert
            Assert.Equal(serverCode, result.server_code);
            _mockServerRepository.Verify(x => x.GetByCodeAsync(It.IsAny<Expression<Func<ServerEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task EnsureStatusExists_Should_Throw_Exception_When_Status_Not_Found()
        {
            // Arrange
            var statusId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockServerService.InsertAsync(new ServerEntity { status_id = statusId }));

            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
        }

        [Fact]
        public async Task GetByTypeAsync_Should_Return_Servers_By_TypeId()
        {
            // Arrange
            var typeId = Guid.NewGuid();
            var servers = new List<ServerEntity>
            {
                new ServerEntity { id = Guid.NewGuid(), type_id = typeId },
                new ServerEntity { id = Guid.NewGuid(), type_id = typeId }
            };

            _mockServerRepository.Setup(x => x.GetByTypeAsync(It.IsAny<Expression<Func<ServerEntity, bool>>>()))
                .ReturnsAsync(servers);

            // Act
            var result = await _mockServerService.GetByTypeAsync(typeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(servers.Count, result.Count());
            Assert.All(result, server => Assert.Equal(typeId, server.type_id));
            _mockServerRepository.Verify(x => x.GetByTypeAsync(It.IsAny<Expression<Func<ServerEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Throw_OrchestratorArgumentException_When_Code_Is_Not_Unique()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var server = new ServerEntity
            {
                id = Guid.NewGuid(),
                server_code = "ExistingCode",
                status_id = statusId
            };

            var status = new StatusEntity { id = statusId };
            var existingServer = new ServerEntity
            {
                id = Guid.NewGuid(),
                server_code = "ExistingCode" // Simulamos un código ya existente
            };

            // Simulamos que el estado es encontrado
            _mockStatusService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(status);

            // Simulamos la generación del código
            _mockCodeConfiguratorService.Setup(x => x.GenerateCodeAsync(It.IsAny<Prefix>()))
                .ReturnsAsync("ExistingCode");

            // Simulamos que el código ya existe en el repositorio
            _mockServerRepository.Setup(x => x.GetByCodeAsync(It.IsAny<Expression<Func<ServerEntity, bool>>>()))
                .ReturnsAsync(existingServer);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(async () =>
            {
                await _mockServerService.InsertAsync(server); // Llamamos al método InsertAsync que debería fallar
            });

            // Verificamos los detalles de la excepción
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
            Assert.Equal("ExistingCode", exception.Details.Data);

            // Verificamos que el código haya sido buscado una vez con el valor correcto
            _mockServerRepository.Verify(x => x.GetByCodeAsync(It.IsAny<Expression<Func<ServerEntity, bool>>>()), Times.Once);

            // Verificamos que no se intentó insertar el repositorio
            _mockServerRepository.Verify(x => x.InsertAsync(It.IsAny<ServerEntity>()), Times.Never);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnPaginatedAdapters_WhenCalled()
        {
            // Arrange
            var paginatedModel = new PaginatedModel
            {
                First = 0,
                Rows = 10,
                Sort_field = "",
                Sort_order = SortOrdering.Ascending,
                Search = "Server",
                activeOnly = true
            };

            var servers = new List<ServerResponseModel>
            {
                new ServerResponseModel { server_name = "Server 1" },
                new ServerResponseModel { server_name = "Server 2" }
            };

            _mockServerRepository.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<ServerEntity>>()))
                .ReturnsAsync(servers);

            // Act
            var result = await _mockServerService.GetAllPaginatedAsync(paginatedModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockServerRepository.Verify(x => x.GetAllAsync(It.IsAny<ISpecification<ServerEntity>>()), Times.Once);
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
                Search = "Server",
                activeOnly = true
            };

            _mockServerRepository.Setup(x => x.GetTotalRows(It.IsAny<ISpecification<ServerEntity>>()))
                .ReturnsAsync(count);

            // Act
            var result = await _mockServerService.GetTotalRowsAsync(paginatedModel);

            // Assert
            Assert.Equal(count, result);
            _mockServerRepository.Verify(x => x.GetTotalRows(It.IsAny<ISpecification<ServerEntity>>()), Times.Once);
        }
    }
}
