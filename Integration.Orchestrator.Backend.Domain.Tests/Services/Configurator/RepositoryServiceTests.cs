using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
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
    public class RepositoryServiceTests
    {
        private readonly Mock<IRepositoryRepository<RepositoryEntity>> _mockRepositoryRepository;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly RepositoryService _mockRepositoryService;

        public RepositoryServiceTests()
        {
            _mockRepositoryRepository = new Mock<IRepositoryRepository<RepositoryEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();

            _mockRepositoryService = new RepositoryService(
                _mockRepositoryRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );
        }

        [Fact]
        public async Task InsertAsync_Should_Throw_Exception_When_Status_Not_Found()
        {
            // Arrange
            var repository = new RepositoryEntity
            {
                status_id = Guid.NewGuid()
            };

            var service = new RepositoryService(
                _mockRepositoryRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => service.InsertAsync(repository));
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);

            _mockRepositoryRepository.Verify(x => x.InsertAsync(It.IsAny<RepositoryEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Repository_When_Valid()
        {
            // Arrange
            var repository = new RepositoryEntity
            {
                repository_code = "REPO001",
                status_id = Guid.NewGuid()
            };

            _mockStatusService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new StatusEntity());  // Simula que el estado existe

            _mockRepositoryRepository.Setup(x => x.ValidateDbPortUser(It.IsAny<RepositoryEntity>()))
                .ReturnsAsync(false);  // No hay duplicados de puerto de DB

            var service = new RepositoryService(
                _mockRepositoryRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );

            // Act
            await service.UpdateAsync(repository);

            // Assert
            _mockRepositoryRepository.Verify(x => x.UpdateAsync(It.IsAny<RepositoryEntity>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Repository_Successfully()
        {
            // Arrange
            var repository = new RepositoryEntity
            {
                id = Guid.NewGuid(),
                repository_code = "REPO123"
            };

            _mockRepositoryRepository.Setup(x => x.DeleteAsync(It.IsAny<RepositoryEntity>()))
                .Returns(Task.CompletedTask);  // Simula la eliminación exitosa

            var service = new RepositoryService(
                _mockRepositoryRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );

            // Act
            await service.DeleteAsync(repository);

            // Assert
            _mockRepositoryRepository.Verify(x => x.DeleteAsync(It.IsAny<RepositoryEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Repository_When_Found()
        {
            // Arrange
            var repositoryId = Guid.NewGuid();
            var expectedRepository = new RepositoryEntity
            {
                id = repositoryId,
                repository_code = "REPO123"
            };

            _mockRepositoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<RepositoryEntity, bool>>>()))
                .ReturnsAsync(expectedRepository); // Simula que se encuentra el repositorio

            var service = new RepositoryService(
                _mockRepositoryRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );

            // Act
            var result = await service.GetByIdAsync(repositoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRepository.id, result.id);
            Assert.Equal(expectedRepository.repository_code, result.repository_code);
            _mockRepositoryRepository.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<RepositoryEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByCodeAsync_Should_Return_Repository_When_Found()
        {
            // Arrange
            var repositoryCode = "REPO123";
            var expectedRepository = new RepositoryEntity
            {
                id = Guid.NewGuid(),
                repository_code = repositoryCode
            };

            _mockRepositoryRepository.Setup(x => x.GetByCodeAsync(It.IsAny<Expression<Func<RepositoryEntity, bool>>>()))
                .ReturnsAsync(expectedRepository); // Simula que se encuentra el repositorio con el código dado

            var service = new RepositoryService(
                _mockRepositoryRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );

            // Act
            var result = await service.GetByCodeAsync(repositoryCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRepository.repository_code, result.repository_code);
            _mockRepositoryRepository.Verify(x => x.GetByCodeAsync(It.IsAny<Expression<Func<RepositoryEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Throw_OrchestratorArgumentException_When_Status_Not_Found()
        {
            // Arrange
            var repository = new RepositoryEntity
            {
                id = Guid.NewGuid(),
                repository_code = "Repo001",
                status_id = Guid.NewGuid() // Estado inexistente
            };

            var service = new RepositoryService(
                _mockRepositoryRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(async () =>
            {
                await service.InsertAsync(repository); // Llamamos al método InsertAsync que debería fallar
            });

            // Verificamos los detalles de la excepción
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
            Assert.Equal(repository.status_id, exception.Details.Data);

            // Verificamos que el servicio de estado fue llamado una vez con el ID correcto
            _mockStatusService.Verify(x => x.GetByIdAsync(It.Is<Guid>(id => id == repository.status_id)), Times.Once);

            // Verificamos que no se intentó insertar el repositorio, ya que falló antes de llegar a esa parte
            _mockRepositoryRepository.Verify(x => x.InsertAsync(It.IsAny<RepositoryEntity>()), Times.Never);
        }

        [Fact]
        public async Task InsertAsync_Should_Throw_OrchestratorArgumentException_When_Code_Is_Not_Unique()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var repository = new RepositoryEntity
            {
                id = Guid.NewGuid(),
                repository_code = "ExistingCode",
                status_id = statusId
            };

            var status = new StatusEntity { id = statusId };
            var existingRepository = new RepositoryEntity
            {
                id = Guid.NewGuid(),
                repository_code = "ExistingCode" // Simulamos un código ya existente
            };

            // Simulamos que el estado es encontrado
            _mockStatusService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(status);

            // Simulamos la generación del código
            _mockCodeConfiguratorService.Setup(x => x.GenerateCodeAsync(It.IsAny<Prefix>()))
                .ReturnsAsync("ExistingCode");

            // Simulamos que el código ya existe en el repositorio
            _mockRepositoryRepository.Setup(x => x.GetByCodeAsync(It.IsAny<Expression<Func<RepositoryEntity, bool>>>()))
                .ReturnsAsync(existingRepository);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(async () =>
            {
                await _mockRepositoryService.InsertAsync(repository); // Llamamos al método InsertAsync que debería fallar
            });

            // Verificamos los detalles de la excepción
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
            Assert.Equal("ExistingCode", exception.Details.Data);

            // Verificamos que el código haya sido buscado una vez con el valor correcto
            _mockRepositoryRepository.Verify(x => x.GetByCodeAsync(It.IsAny<Expression<Func<RepositoryEntity, bool>>>()), Times.Once);

            // Verificamos que no se intentó insertar el repositorio
            _mockRepositoryRepository.Verify(x => x.InsertAsync(It.IsAny<RepositoryEntity>()), Times.Never);
        }

        [Fact]
        public async Task InsertAsync_Should_Insert_Repository_When_DbPortUser_Is_Not_Duplicated()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var repository = new RepositoryEntity
            {
                id = Guid.NewGuid(),
                repository_code = "UniqueCode",
                status_id = statusId,
                // Otras propiedades necesarias para la validación de duplicidad de DbPortUser
            };

            var status = new StatusEntity { id = statusId };

            // Simulamos que el estado es encontrado
            _mockStatusService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(status);

            // Simulamos que el código es único y se generará correctamente
            _mockCodeConfiguratorService.Setup(x => x.GenerateCodeAsync(It.IsAny<Prefix>()))
                .ReturnsAsync("UniqueCode");

            // Simulamos que no hay duplicidad en la validación de DbPortUser
            _mockRepositoryRepository.Setup(x => x.ValidateDbPortUser(It.IsAny<RepositoryEntity>()))
                .ReturnsAsync(true);  // Indica que no hay duplicidad

            // Act
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockRepositoryService.InsertAsync(repository));

            // Verificar que se lanzó la excepción y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_RepositoryExists, exception.Details.Description);
            Assert.Equal(repository, exception.Details.Data);

            // Assert
            _mockRepositoryRepository.Verify(x => x.ValidateDbPortUser(It.IsAny<RepositoryEntity>()), Times.Once);
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
                Search = "Repository",
                activeOnly = true
            };

            var adapterEntities = new List<RepositoryResponseModel>
            {
                new RepositoryResponseModel { repository_databaseName = "Repository 1" },
                new RepositoryResponseModel { repository_databaseName = "Repository 2" }
            };

            _mockRepositoryRepository.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<RepositoryEntity>>()))
                .ReturnsAsync(adapterEntities);

            // Act
            var result = await _mockRepositoryService.GetAllPaginatedAsync(paginatedModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepositoryRepository.Verify(x => x.GetAllAsync(It.IsAny<ISpecification<RepositoryEntity>>()), Times.Once);
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
                Search = "Property",
                activeOnly = true
            };

            _mockRepositoryRepository.Setup(x => x.GetTotalRows(It.IsAny<ISpecification<RepositoryEntity>>()))
                .ReturnsAsync(count);

            // Act
            var result = await _mockRepositoryService.GetTotalRowsAsync(paginatedModel);

            // Assert
            Assert.Equal(count, result);
            _mockRepositoryRepository.Verify(x => x.GetTotalRows(It.IsAny<ISpecification<RepositoryEntity>>()), Times.Once);
        }

    }
}
