using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Specifications
{
    public class SynchronizationSpecificationTests
    {
        [Fact]
        public void Constructor_ShouldSetCorrectCriteria_WhenSearchIsProvided()
        {
            var paginatedModel = new PaginatedModel
            {
                Search = "",
                First = 1,
                Rows = 10,
                Sort_field = "status"
            };

            var specification = new SynchronizationSpecification(paginatedModel);

            var criteria = specification.Criteria;
            var compiledCriteria = criteria.Compile();
            var testEntity = new SynchronizationEntity
            {
                status_id = Guid.NewGuid(),
                synchronization_observations = "some observations"
            };

            Assert.True(compiledCriteria(testEntity));
        }

        [Fact]
        public void Constructor_ShouldSetCorrectCriteria_WhenSearchIsNotProvided()
        {
            var paginatedModel = new PaginatedModel
            {
                First = 1,
                Rows = 10,
                Sort_field = ""
            };

            var specification = new SynchronizationSpecification(paginatedModel);

            var criteria = specification.Criteria;
            var compiledCriteria = criteria.Compile();
            var testEntity = new SynchronizationEntity
            {
                status_id = Guid.NewGuid(),
                synchronization_observations = "any observations"
            };

            Assert.True(compiledCriteria(testEntity));
        }

        [Fact]
        public void Constructor_ShouldSetCorrectPagination()
        {
            var paginatedModel = new PaginatedModel
            {
                First = 2,
                Rows = 10,
                Sort_field = ""
            };

            var specification = new SynchronizationSpecification(paginatedModel);

            Assert.Equal(10, specification.Skip);
            Assert.Equal(10, specification.Limit);
        }

        [Fact]
        public void Constructor_ShouldSetCorrectOrdering_WhenSortByIsProvided()
        {
            var paginatedModel = new PaginatedModel
            {
                Sort_field = nameof(SynchronizationEntity.status_id),
                Sort_order = SortOrdering.Ascending,
                First = 1,
                Rows = 10
            };

            var specification = new SynchronizationSpecification(paginatedModel);

            Assert.NotNull(specification.OrderBy);
            Assert.Null(specification.OrderByDescending);
        }

        [Fact]
        public void Constructor_ShouldSetCorrectOrdering_WhenSortByIsProvidedAndDescending()
        {
            var paginatedModel = new PaginatedModel
            {
                Sort_field = nameof(SynchronizationEntity.status_id),
                Sort_order = SortOrdering.Descending,
                First = 1,
                Rows = 10
            };

            var specification = new SynchronizationSpecification(paginatedModel);

            Assert.Null(specification.OrderBy);
            Assert.NotNull(specification.OrderByDescending);
        }        
    }
}
