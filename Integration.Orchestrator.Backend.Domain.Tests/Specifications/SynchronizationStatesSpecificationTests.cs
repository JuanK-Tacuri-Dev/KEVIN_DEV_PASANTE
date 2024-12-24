using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Tests.Specifications
{
    public class SynchronizationStatesSpecificationTests
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

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            var criteria = specification.Criteria;
            var compiledCriteria = criteria.Compile();
            var testEntity = new SynchronizationStatusEntity
            {
                id = Guid.NewGuid(),
                synchronization_status_key = "Cancelado",
                synchronization_status_text = "canceled",
                synchronization_status_color = "F77D7D",
                synchronization_status_background = "#E2F7E2"
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

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            var criteria = specification.Criteria;
            var compiledCriteria = criteria.Compile();
            var testEntity = new SynchronizationStatusEntity
            {
                id = Guid.NewGuid(),
                synchronization_status_key = "Cancelado",
                synchronization_status_text = "canceled",
                synchronization_status_color = "F77D7D",
                synchronization_status_background = "#E2F7E2"
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

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            Assert.Equal(2, specification.Skip);
            Assert.Equal(10, specification.Limit);
        }

        [Fact]
        public void Constructor_ShouldSetCorrectOrdering_WhenSortByIsProvided()
        {
            var paginatedModel = new PaginatedModel
            {
                Sort_field = nameof(SynchronizationStatusEntity.synchronization_status_key),
                Sort_order = SortOrdering.Ascending,
                First = 1,
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
                Sort_field = nameof(SynchronizationStatusEntity.synchronization_status_key),
                Sort_order = SortOrdering.Descending,
                First = 1,
                Rows = 10
            };

            var specification = new SynchronizationStatesSpecification(paginatedModel);

            Assert.NotNull(specification.OrderBy);
            Assert.Null(specification.OrderByDescending);
        }
    }
}
