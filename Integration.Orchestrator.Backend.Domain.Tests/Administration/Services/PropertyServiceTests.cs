using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Administration.Services
{
    public class PropertyServiceTests
    {
        private readonly Mock<IPropertyRepository<PropertyEntity>> _mockPropertyRepository;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly PropertyService _mockPropertyService;

        public PropertyServiceTests()
        {
            _mockPropertyRepository = new Mock<IPropertyRepository<PropertyEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();

            _mockPropertyService = new PropertyService(
                _mockPropertyRepository.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );
        }

        [Fact]
        public async Task InsertAsync_ShouldInsertProperty_WhenValid()
        {
            // Arrange
            var property = new PropertyEntity { property_name = "TestProperty", status_id = Guid.NewGuid(), entity_id = Guid.NewGuid() };

            _mockStatusService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new StatusEntity { status_key = "Active" });

            _mockCodeConfiguratorService.Setup(s => s.GenerateCodeAsync(It.IsAny<Prefix>()))
                .ReturnsAsync("PR001");

            _mockPropertyRepository.Setup(r => r.InsertAsync(It.IsAny<PropertyEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mockPropertyService.InsertAsync(property);

            // Assert
            _mockPropertyRepository.Verify(r => r.InsertAsync(It.IsAny<PropertyEntity>()), Times.Once);
            Assert.Equal("PR001", property.property_code);
        }

        [Fact]
        public async Task InsertAsync_ShouldThrowArgumentException_WhenPropertyExists()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var code = "P001";
            var property = new PropertyEntity 
            { 
                property_name = "TestProperty", 
                property_code = code,
                entity_id = Guid.NewGuid(),
                status_id = statusId
            };

            _mockPropertyRepository.Setup(r => r.ValidateNameAndEntity(It.IsAny<PropertyEntity>()))
                .ReturnsAsync(true);

            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockPropertyService.InsertAsync(property));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_PropertyExists, exception.Details.Description);
            
        }

        [Fact]
        public async Task InsertAsync_ShouldThrowOrchestratorArgumentException_WhenStatusNotFound()
        {
            // Arrange
            var property = new PropertyEntity { status_id = Guid.NewGuid() };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockPropertyService.InsertAsync(property));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
            Assert.Equal(property.status_id, exception.Details.Data);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProperty_WhenValid()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var property = new PropertyEntity 
            { 
                property_name = "UpdatedProperty",
                status_id = statusId,
            };

            _mockPropertyRepository.Setup(r => r.UpdateAsync(It.IsAny<PropertyEntity>()))
                .Returns(Task.CompletedTask);

            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            // Act
            await _mockPropertyService.UpdateAsync(property);

            // Assert
            _mockPropertyRepository.Verify(r => r.UpdateAsync(It.IsAny<PropertyEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowOrchestratorArgumentException_WhenDuplicatePropertyExists()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var property = new PropertyEntity 
            { 
                property_name = "DuplicateProperty",
                status_id = statusId,
            };

            _mockPropertyRepository.Setup(r => r.ValidateNameAndEntity(It.IsAny<PropertyEntity>()))
                .ReturnsAsync(true); // Simulamos que la propiedad ya existe

            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _mockPropertyService.UpdateAsync(property));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code); 
            Assert.Equal(AppMessages.Domain_PropertyExists, exception.Details.Description);      
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProperty_WhenValid()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var property = new PropertyEntity 
            { 
                property_name = "PropertyToDelete",
                status_id = statusId
            };

            _mockPropertyRepository.Setup(r => r.DeleteAsync(It.IsAny<PropertyEntity>()))
                .Returns(Task.CompletedTask);

            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            // Act
            await _mockPropertyService.DeleteAsync(property);

            // Assert
            _mockPropertyRepository.Verify(r => r.DeleteAsync(It.IsAny<PropertyEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProperty_WhenPropertyExists()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var property = new PropertyEntity { property_name = "ExistingProperty" };

            _mockPropertyRepository.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<PropertyEntity,bool>>>()))
                .ReturnsAsync(property);

            // Act
            var result = await _mockPropertyService.GetByIdAsync(propertyId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ExistingProperty", result.property_name);
        }

        [Fact]
        public async Task GetByEntityIdAsync_ShouldReturnProperties_WhenPropertiesExist()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var properties = new List<PropertyEntity>
            {
                new PropertyEntity { property_name = "Property1", entity_id = entityId },
                new PropertyEntity { property_name = "Property2", entity_id = entityId }
            };

            _mockPropertyRepository.Setup(r => r.GetByEntityAsync(It.IsAny<Expression<Func<PropertyEntity, bool>>>()))
                .ReturnsAsync(properties); // Simulamos que hay propiedades encontradas

            // Act
            var result = await _mockPropertyService.GetByEntityIdAsync(entityId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.property_name == "Property1");
            Assert.Contains(result, p => p.property_name == "Property2");
            _mockPropertyRepository.Verify(r => r.GetByEntityAsync(It.IsAny<Expression<Func<PropertyEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByTypeIdAsync_ShouldReturnProperties_WhenPropertiesExist()
        {
            // Arrange
            var typeId = Guid.NewGuid();
            var expectedProperties = new List<PropertyEntity>
            {
                new PropertyEntity { type_id = typeId, property_code = "PROP001" },
                new PropertyEntity { type_id = typeId, property_code = "PROP002" }
            };

            _mockPropertyRepository
                .Setup(repo => repo.GetByTypeAsync(It.IsAny<Expression<Func<PropertyEntity, bool>>>()))
                .ReturnsAsync(expectedProperties);

            // Act
            var result = await _mockPropertyService.GetByTypeIdAsync(typeId);

            // Assert
            Assert.NotNull(result); // Verifica que el resultado no sea null
            Assert.Equal(expectedProperties.Count, result.Count()); // Verifica que se hayan retornado las propiedades esperadas
            Assert.Equal(expectedProperties, result); // Verifica el primer elemento
        }

        [Fact]
        public async Task InsertAsync_ShouldFail_WhenCodeIsNotUnique()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var property = new PropertyEntity
            {
                property_code = "PRTY123", // Simulación de código que será verificado como no único
                property_name = "Test Property",
                status_id = statusId,
                entity_id = Guid.NewGuid()
            };

            // Simulamos que el servicio de código genera un código que ya existe
            _mockCodeConfiguratorService
                .Setup(svc => svc.GenerateCodeAsync(It.IsAny<Prefix>()))
                .ReturnsAsync("PRTY123"); // Código que se va a generar

            // Simulamos que el código ya existe en la base de datos
            _mockPropertyRepository
                .Setup(repo => repo.GetByCodeAsync(It.IsAny<Expression<Func<PropertyEntity, bool>>>()))
                .ReturnsAsync(property); // Retorna una propiedad que ya tiene ese código

            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() =>
                _mockPropertyService.InsertAsync(property)
            );

            // Verificamos que el mensaje de la excepción sea el correcto
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
        }

        [Fact]
        public async Task InsertAsync_ShouldFail_WhenPropertyIsDuplicate()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var property = new PropertyEntity
            {
                property_code = "PRTY123",
                property_name = "Test Property",
                entity_id = Guid.NewGuid(),
                status_id = statusId
            };

            // Simulamos que el repositorio indica que ya existe una propiedad con el mismo nombre y entity_id
            _mockPropertyRepository
                .Setup(repo => repo.ValidateNameAndEntity(It.IsAny<PropertyEntity>()))
                .ReturnsAsync(true); // Retorna true indicando que ya existe un duplicado

            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() =>
                _mockPropertyService.InsertAsync(property)
            );

            // Verificamos que el mensaje de la excepción sea el correcto
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_PropertyExists, exception.Details.Description);
        }

    }
}
