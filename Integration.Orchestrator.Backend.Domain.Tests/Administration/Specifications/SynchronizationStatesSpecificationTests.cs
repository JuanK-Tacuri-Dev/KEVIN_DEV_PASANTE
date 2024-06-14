using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Specifications
{
    public class SynchronizationStatesSpecificationTests
    {
        [Fact]
        public void Constructor_ShouldSetCorrectCriteria_WhenSearchIsProvided()
        {
            var paginatedModel = new PaginatedModel
            {
                Search = "",
                Page = 1,
                Rows = 10,
                SortBy = "status"
            };

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            var criteria = specification.Criteria;
            var compiledCriteria = criteria.Compile();
            var testEntity = new SynchronizationStatesEntity
            {
                id = Guid.NewGuid(),
                name = "Cancelado",
                code = "canceled",
                color = "F77D7D"
            };

            Assert.True(compiledCriteria(testEntity));
        }

        [Fact]
        public void Constructor_ShouldSetCorrectCriteria_WhenSearchIsNotProvided()
        {
            var paginatedModel = new PaginatedModel
            {
                Page = 1,
                Rows = 10,
                SortBy = ""
            };

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            var criteria = specification.Criteria;
            var compiledCriteria = criteria.Compile();
            var testEntity = new SynchronizationStatesEntity
            {
                id = Guid.NewGuid(),
                name = "Cancelado",
                code = "canceled",
                color = "F77D7D"
            };

            Assert.True(compiledCriteria(testEntity));
        }

        [Fact]
        public void Constructor_ShouldSetCorrectPagination()
        {
            var paginatedModel = new PaginatedModel
            {
                Page = 2,
                Rows = 10,
                SortBy = ""
            };

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            Assert.Equal(10, specification.Skip);
            Assert.Equal(10, specification.Limit);
        }

        [Fact]
        public void Constructor_ShouldSetCorrectOrdering_WhenSortByIsProvided()
        {
            var paginatedModel = new PaginatedModel
            {
                SortBy = nameof(SynchronizationStatesEntity.name),
                SortOrder = SortOrdering.Ascending,
                Page = 1,
                Rows = 10
            };

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            Assert.NotNull(specification.OrderBy);
            Assert.Null(specification.OrderByDescending);
        }

        [Fact]
        public void Constructor_ShouldSetCorrectOrdering_WhenSortByIsProvidedAndDescending()
        {
            var paginatedModel = new PaginatedModel
            {
                SortBy = nameof(SynchronizationStatesEntity.name),
                SortOrder = SortOrdering.Descending,
                Page = 1,
                Rows = 10
            };

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            Assert.Null(specification.OrderBy);
            Assert.NotNull(specification.OrderByDescending);
        }        
    }
}
