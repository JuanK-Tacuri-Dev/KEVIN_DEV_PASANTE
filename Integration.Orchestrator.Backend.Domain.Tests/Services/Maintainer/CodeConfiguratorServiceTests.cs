using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Services.Maintainer
{
    public class CodeConfiguratorServiceTests
    {
        private readonly Mock<ICodeConfiguratorRepository<CodeConfiguratorEntity>> _mockCodeConfiguratorRepository;
        private readonly Mock<ICatalogRepository<CatalogEntity>> _mockCatalogRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService;

        public CodeConfiguratorServiceTests()
        {
            _mockCodeConfiguratorRepository = new Mock<ICodeConfiguratorRepository<CodeConfiguratorEntity>>();
            _mockCatalogRepository = new Mock<ICatalogRepository<CatalogEntity>>();

            _codeConfiguratorService = new CodeConfiguratorService(
                _mockCodeConfiguratorRepository.Object,
                _mockCatalogRepository.Object
            );
        }

        [Fact]
        public async Task GenerateCodeAsync_Should_Return_GeneratedCode_When_Prefix_Is_Found()
        {
            // Arrange
            var prefix = Prefix.Catalog; // Reemplazar con el valor real de Prefix
            var catalogEntity = new CatalogEntity { catalog_value = "ABC" };
            var codeConfiguratorEntity = new CodeConfiguratorEntity
            {
                id = Guid.NewGuid(),
                type = (int)prefix,
                value_text = "S",
                value_number = 1
            };

            _mockCatalogRepository.Setup(x => x.GetByCodeAsync(It.IsAny<Expression<Func<CatalogEntity, bool>>>()))
                .ReturnsAsync(catalogEntity);

            _mockCodeConfiguratorRepository.Setup(x => x.GetByTypeAsync((int)prefix))
                .ReturnsAsync(codeConfiguratorEntity);

            _mockCodeConfiguratorRepository.Setup(x => x.IncrementModuleSequenceAsync(It.IsAny<CodeConfiguratorEntity>()))
                .ReturnsAsync(new CodeConfiguratorEntity { id = Guid.NewGuid(), type = (int)prefix, value_text = "S", value_number = 1 });

            // Act
            var result = await _codeConfiguratorService.GenerateCodeAsync(prefix);

            // Assert
            Assert.Equal("S001", result); // El formato debería ajustarse según la lógica
            _mockCatalogRepository.Verify(x => x.GetByCodeAsync(It.IsAny<Expression<Func<CatalogEntity, bool>>>()), Times.Once);
            _mockCodeConfiguratorRepository.Verify(x => x.GetByTypeAsync((int)prefix), Times.Once);
            _mockCodeConfiguratorRepository.Verify(x => x.IncrementModuleSequenceAsync(It.IsAny<CodeConfiguratorEntity>()), Times.Once);
        }

        [Fact]
        public async Task GenerateCodeAsync_Should_Throw_Exception_When_UnexpectedErrorOccurs()
        {
            // Arrange
            var prefix = Prefix.Catalog; // Reemplazar con el valor real de Prefix
            var catalogEntity = new CatalogEntity { catalog_value = "ABC" };
            var codeConfiguratorEntity = new CodeConfiguratorEntity
            {
                id = Guid.NewGuid(),
                type = (int)prefix,
                value_text = "S",
                value_number = 1
            };

            // Act
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _codeConfiguratorService.GenerateCodeAsync(prefix));

            // Assert
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(string.Format(AppMessages.Domain_NonParameterizedPrefix, prefix.ToString()), exception.Details.Description);
        }

    }
}
