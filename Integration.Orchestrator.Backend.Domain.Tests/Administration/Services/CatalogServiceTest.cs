using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class CatalogServiceTest
    {
        private readonly Mock<ICatalogRepository<CatalogEntity>> _mockRepo;
        private readonly CatalogService _service;
        public CatalogServiceTest()
        {
            _mockRepo = new Mock<ICatalogRepository<CatalogEntity>>();
            _service = new CatalogService(_mockRepo.Object);
        }
        [Fact]
        public async Task DeleteAsync()
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
        public async Task GetAllPaginatedAsync()
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
        public async Task GetByFatherAsync()
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
        public async Task GetByIdAsync()
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
        public async Task GetTotalRowsAsync()
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
        public async Task InsertAsync()
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
            await _service.InsertAsync(catalog);

            _mockRepo.Verify(repo => repo.InsertAsync(catalog), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync()
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
            await _service.UpdateAsync(catalog);
            _mockRepo.Verify(repo => repo.UpdateAsync(catalog), Times.Once);
        }
    }
}
