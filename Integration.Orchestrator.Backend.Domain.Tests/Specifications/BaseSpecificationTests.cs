using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Specifications
{
    public class BaseSpecificationTests
    {
        [Fact]
        public void GetAll_ShouldReturnTrue()
        {
            // Arrange
            var expression = BaseSpecification<object>.GetAll();
            var func = expression.Compile();

            // Act
            var result = func(new object());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetByUuid_ShouldReturnCorrectExpression()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            Expression<Func<TestEntity, Guid>> propertySelector = e => e.Id;

            var expression = BaseSpecification<TestEntity>.GetByUuid(propertySelector, uuid);
            var func = expression.Compile();

            // Act & Assert
            var matchingEntity = new TestEntity { Id = uuid };
            var nonMatchingEntity = new TestEntity { Id = Guid.NewGuid() };

            Assert.True(func(matchingEntity));
            Assert.False(func(nonMatchingEntity));
        }

        private class TestEntity
        {
            public Guid Id { get; set; }
        }
    }
}
