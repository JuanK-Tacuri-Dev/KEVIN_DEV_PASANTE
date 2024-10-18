using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
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
    public class CatalogServiceTest
    {
        private readonly Mock<ICatalogRepository<CatalogEntity>> _mockRepo;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly CatalogService _service;
        public CatalogServiceTest()
        {
            _mockRepo = new Mock<ICatalogRepository<CatalogEntity>>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();
            _service = new CatalogService(_mockRepo.Object, _mockStatusService.Object);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteAsync()
        {
            var entity = new CatalogEntity
            {
                catalog_code = 10,
                catalog_name = "Catlog",
                catalog_value = "value",
                father_code = 1,
                catalog_detail = "details of catalog",
                status_id = Guid.NewGuid()
            };
            await _service.DeleteAsync(entity);
            _mockRepo.Verify(repo => repo.DeleteAsync(entity), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnPaginatedCatalogEntities()
        {
            var paginatedModel = new PaginatedModel()
            {
                First = 1,
                Rows = 1,
                Search = "",
                Sort_field = "",
                Sort_order = Commons.SortOrdering.Ascending
            };
            var catalog = new CatalogEntity
            {
                catalog_code = 10,
                catalog_name = "Catlog",
                catalog_value = "value",
                father_code = 1,
                catalog_detail = "details of catalog",
                status_id = Guid.NewGuid()
            };
            var catalogs = new List<CatalogEntity> { catalog };
            var spec = new CatalogSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<ISpecification<CatalogEntity>>())).ReturnsAsync(catalogs);

            var result = await _service.GetAllPaginatedAsync(paginatedModel);
            List<CatalogEntity> r = result.ToList();
            Assert.Equal(catalogs, result);
            _mockRepo.Verify(repo => repo.GetAllAsync(It.IsAny<CatalogSpecification>()), Times.Once);
        }

        [Fact]
        public async Task GetByFatherAsync_ShouldReturnCatalogEntitiesByFatherCode()
        {
            var father_code = 1;
            var catalog = new CatalogEntity
            {
                catalog_code = 10,
                catalog_name = "Catlog",
                catalog_value = "value",
                father_code = father_code,
                catalog_detail = "details of catalog",
                status_id = Guid.NewGuid()
            };
            var catalogs = new List<CatalogEntity> { catalog };
            var specification = CatalogSpecification.GetByFatherExpression(father_code);
            _mockRepo.Setup(repo => repo.GetByFatherAsync(It.IsAny<Expression<Func<CatalogEntity, bool>>>()))
                     .ReturnsAsync(catalogs);

            var result = await _service.GetByFatherAsync(father_code);

            Assert.Equal(catalogs, result);
            /*_mockRepo.Verify(repo => repo.GetByIdAsync(It.Is<Expression<Func<CatalogEntity, bool>>>(expr =>
                expr.Compile()(catalog))), Times.Once);*/
            _mockRepo.Verify(repo => repo.GetByFatherAsync(It.IsAny<Expression<Func<CatalogEntity, bool>>>()), Times.Once);

        }
        
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCatalogEntity_WhenIdExists()
        {
            var id = Guid.NewGuid();
            var catalog = new CatalogEntity
            {
                catalog_code = 10,
                catalog_name = "Catlog",
                catalog_value = "value",
                father_code = 1,
                catalog_detail = "details of catalog",
                status_id = Guid.NewGuid()
            };
            var expression = CatalogSpecification.GetByIdExpression(id);
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<CatalogEntity, bool>>>()))
                .ReturnsAsync(catalog);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(catalog, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<CatalogEntity, bool>>>()),
                Times.Once);
        }
        
        [Fact]
        public async Task GetTotalRowsAsync_ShouldReturnTotalNumberOfRows()
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
            var spec = new CatalogSpecification(paginatedModel);
            _mockRepo.Setup(repo => repo.GetTotalRows(It.IsAny<ISpecification<CatalogEntity>>())).ReturnsAsync(totalRows);
            var result = await _service.GetTotalRowsAsync(paginatedModel);
            Assert.Equal(totalRows, result);
            _mockRepo.Verify(repo => repo.GetTotalRows(It.IsAny<CatalogSpecification>()), Times.Once);
        }
        
        [Fact]
        public async Task InsertAsync_ShouldInsertCatalogEntity_WhenStatusExists()
        {
            var catalog = new CatalogEntity
            {
                catalog_code = 10,
                catalog_name = "Catlog",
                catalog_value = "value",
                father_code = 1,
                catalog_detail = "details of catalog",
                status_id = Guid.NewGuid()
            };
            _mockStatusService.Setup(repo => repo.GetByIdAsync(catalog.status_id)).ReturnsAsync(new StatusEntity { });
            await _service.InsertAsync(catalog);

            _mockRepo.Verify(repo => repo.InsertAsync(catalog), Times.Once);
        }
        
        [Fact]
        public async Task UpdateAsync_ShouldUpdateCatalogEntity_WhenStatusExists()
        {
            var catalog = new CatalogEntity
            {
                catalog_code = 10,
                catalog_name = "Catlog",
                catalog_value = "value",
                father_code = 1,
                catalog_detail = "details of catalog",
                status_id = Guid.NewGuid()
            };
            _mockStatusService.Setup(repo => repo.GetByIdAsync(catalog.status_id)).ReturnsAsync(new StatusEntity { });

            await _service.UpdateAsync(catalog);
            _mockRepo.Verify(repo => repo.UpdateAsync(catalog), Times.Once);
        }

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnCatalogEntity_WhenCodeExists()
        {
            // Arrange
            var catalogCode = 10;
            var catalog = new CatalogEntity
            {
                catalog_code = catalogCode,
                catalog_name = "CatalogName",
                catalog_value = "CatalogValue",
                father_code = 1,
                catalog_detail = "Catalog details",
                status_id = Guid.NewGuid()
            };

            // Configura el mock para que retorne la entidad de catálogo cuando se busca por código
            _mockRepo.Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<CatalogEntity, bool>>>()))
                     .ReturnsAsync(catalog);

            // Act
            var result = await _service.GetByCodeAsync(catalogCode);

            // Assert
            Assert.Equal(catalog, result);
            _mockRepo.Verify(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<CatalogEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task EnsureStatusExists_ShouldThrowOrchestratorArgumentException_WhenStatusDoesNotExist()
        {
            // Arrange
            var nonExistentStatusId = Guid.NewGuid(); // ID de estado que no existe
            var catalogEntity = new CatalogEntity
            {
                catalog_code = 10,
                catalog_name = "CatalogName",
                catalog_value = "CatalogValue",
                father_code = 1,
                catalog_detail = "Catalog details",
                status_id = nonExistentStatusId // Asignamos un ID de estado no existente
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(catalogEntity));

            // Verificar que se lanzó la excepción y que contiene los detalles correctos
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
            Assert.Equal(nonExistentStatusId, exception.Details.Data);

            _mockStatusService.Verify(repo => repo.GetByIdAsync(nonExistentStatusId), Times.Once);
        }

    }
}
