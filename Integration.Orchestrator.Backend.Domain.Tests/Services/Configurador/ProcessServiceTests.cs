using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Moq;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Tests.Services.Configurador
{
    public class ProcessServiceTests
    {
        private readonly Mock<IProcessRepository<ProcessEntity>> _mockRepo;
        private readonly Mock<ICodeConfiguratorService> _mockCodeConfiguratorService;
        private readonly Mock<IStatusService<StatusEntity>> _mockStatusService;
        private readonly ProcessService _service;

        public ProcessServiceTests()
        {
            _mockRepo = new Mock<IProcessRepository<ProcessEntity>>();
            _mockCodeConfiguratorService = new Mock<ICodeConfiguratorService>();
            _mockStatusService = new Mock<IStatusService<StatusEntity>>();

            _service = new ProcessService(
                _mockRepo.Object,
                _mockCodeConfiguratorService.Object,
                _mockStatusService.Object
            );
        }

        [Fact]
        public async Task InsertAsync_ShouldInsertProcess_WhenValidProcessProvided()
        {
            // Arrange
            var process = new ProcessEntity
            {
                id = Guid.NewGuid(),
                process_name = "process_name",
                process_code = "Y001",
                process_type_id = Guid.NewGuid(),
                entities = new List<ObjectEntity>
                {
                    new ObjectEntity
                    {
                        id = Guid.NewGuid(),
                        Properties = new List<PropertiesEntity>
                        {
                            new PropertiesEntity
                            {
                                internal_status_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid()
                            }
                        },
                        filters = new List<FiltersEntity>
                        {
                            new FiltersEntity
                            {
                                operator_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid(),
                                value = "value"
                            }
                        }

                    }
                },
                status_id = Guid.NewGuid()
            };

            _mockStatusService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new StatusEntity());

            _mockCodeConfiguratorService.Setup(x => x.GenerateCodeAsync(Prefix.Process))
                .ReturnsAsync("P002");

            _mockRepo.Setup(x => x.InsertAsync(It.IsAny<ProcessEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.InsertAsync(process);

            // Assert
            _mockRepo.Verify(x => x.InsertAsync(It.IsAny<ProcessEntity>()), Times.Once);
            Assert.Equal("P002", process.process_code);
        }

        [Fact]
        public async Task InsertAsync_ShouldThrowException_WhenStatusNotFound()
        {
            // Arrange
            var process = new ProcessEntity
            {
                id = Guid.NewGuid(),
                process_name = "process_name",
                process_code = "Y001",
                process_type_id = Guid.NewGuid(),
                entities = new List<ObjectEntity>
                {
                    new ObjectEntity
                    {
                        id = Guid.NewGuid(),
                        Properties = new List<PropertiesEntity>
                        {
                            new PropertiesEntity
                            {
                                internal_status_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid()
                            }
                        },
                        filters = new List<FiltersEntity>
                        {
                            new FiltersEntity
                            {
                                operator_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid(),
                                value = "value"
                            }
                        }

                    }
                },
                status_id = Guid.NewGuid()
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(process));

            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Application_StatusNotFound, exception.Details.Description);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProcess_WhenValidProcessProvided()
        {
            // Arrange
            var process = new ProcessEntity
            {
                id = Guid.NewGuid(),
                process_name = "process_name",
                process_code = "Y001",
                process_type_id = Guid.NewGuid(),
                entities = new List<ObjectEntity>
                {
                    new ObjectEntity
                    {
                        id = Guid.NewGuid(),
                        Properties = new List<PropertiesEntity>
                        {
                            new PropertiesEntity
                            {
                                internal_status_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid()
                            }
                        },
                        filters = new List<FiltersEntity>
                        {
                            new FiltersEntity
                            {
                                operator_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid(),
                                value = "value"
                            }
                        }

                    }
                },
                status_id = Guid.NewGuid()
            };

            _mockRepo.Setup(x => x.UpdateAsync(It.IsAny<ProcessEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(process);

            // Assert
            _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<ProcessEntity>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProcess_WhenValidProcessProvided()
        {
            // Arrange
            var process = new ProcessEntity
            {
                process_code = "P001",
                status_id = Guid.NewGuid()
            };

            _mockRepo.Setup(x => x.DeleteAsync(It.IsAny<ProcessEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(process);

            // Assert
            _mockRepo.Verify(x => x.DeleteAsync(It.Is<ProcessEntity>(p => p.process_code == "P001")), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProcess_WhenValidIdProvided()
        {
            // Arrange
            var processId = Guid.NewGuid();
            var process = new ProcessEntity
            {
                id = Guid.NewGuid(),
                process_name = "process_name",
                process_code = "Y001",
                process_type_id = Guid.NewGuid(),
                entities = new List<ObjectEntity>
                {
                    new ObjectEntity
                    {
                        id = Guid.NewGuid(),
                        Properties = new List<PropertiesEntity>
                        {
                            new PropertiesEntity
                            {
                                internal_status_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid()
                            }
                        },
                        filters = new List<FiltersEntity>
                        {
                            new FiltersEntity
                            {
                                operator_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid(),
                                value = "value"
                            }
                        }

                    }
                },
                status_id = Guid.NewGuid()
            };

            _mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()))
                .ReturnsAsync(process);

            // Act
            var result = await _service.GetByIdAsync(processId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Y001", result.process_code);
            _mockRepo.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByEntityActiveIdAsync_ShouldReturnProcess_WhenValidIdProvided()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var process = new ProcessEntity
            {
                id = Guid.NewGuid(),
                process_name = "process_name",
                process_code = "Y001",
                process_type_id = Guid.NewGuid(),
                entities = new List<ObjectEntity>
                {
                    new ObjectEntity
                    {
                        id =entityId,
                        Properties = new List<PropertiesEntity>
                        {
                            new PropertiesEntity
                            {
                                internal_status_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid()
                            }
                        },
                        filters = new List<FiltersEntity>
                        {
                            new FiltersEntity
                            {
                                operator_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid(),
                                value = "value"
                            }
                        }

                    }
                },
                status_id = statusId
            };

            _mockRepo.Setup(x => x.GetByEntitiesIdAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()))
                .ReturnsAsync([process]);

            // Act
            var result = await _service.GetByEntityActiveIdAsync(entityId, statusId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            _mockRepo.Verify(x => x.GetByEntitiesIdAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByPropertyActiveIdAsync_ShouldReturnProcess_WhenValidIdProvided()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var process = new ProcessEntity
            {
                id = Guid.NewGuid(),
                process_name = "process_name",
                process_code = "Y001",
                process_type_id = Guid.NewGuid(),
                entities = new List<ObjectEntity>
                {
                    new ObjectEntity
                    {
                        id = Guid.NewGuid(),
                        Properties = new List<PropertiesEntity>
                        {
                            new PropertiesEntity
                            {
                                internal_status_id = Guid.NewGuid(),
                                property_id = propertyId
                            }
                        },
                        filters = new List<FiltersEntity>
                        {
                            new FiltersEntity
                            {
                                operator_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid(),
                                value = "value"
                            }
                        }

                    }
                },
                status_id = statusId
            };

            _mockRepo.Setup(x => x.GetByPropertiesIdAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()))
                .ReturnsAsync([process]);

            // Act
            var result = await _service.GetByPropertyActiveIdAsync(propertyId, statusId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            _mockRepo.Verify(x => x.GetByPropertiesIdAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByConnectionIdAsync_ShouldReturnProcess_WhenValidIdProvided()
        {
            // Arrange
            var connectionId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var process = new ProcessEntity
            {
                id = Guid.NewGuid(),
                process_name = "process_name",
                process_code = "Y001",
                process_type_id = Guid.NewGuid(),
                connection_id = connectionId,
                entities = new List<ObjectEntity>
                {
                    new ObjectEntity
                    {
                        id = Guid.NewGuid(),
                        Properties = new List<PropertiesEntity>
                        {
                            new PropertiesEntity
                            {
                                internal_status_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid()
                            }
                        },
                        filters = new List<FiltersEntity>
                        {
                            new FiltersEntity
                            {
                                operator_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid(),
                                value = "value"
                            }
                        }

                    }
                },
                status_id = statusId
            };

            _mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()))
                .ReturnsAsync(process);

            // Act
            var result = await _service.GetByConnectionIdAsync(connectionId, statusId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(process, result);
            _mockRepo.Verify(x => x.GetByIdAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()), Times.Once);
        }
        
        [Fact]
        public async Task GetByCodeAsync_ShouldReturnProcess_WhenValidCodeProvided()
        {
            // Arrange
            var processCode = "Y001";
            var process = new ProcessEntity
            {
                id = Guid.NewGuid(),
                process_name = "process_name",
                process_code = processCode,
                process_type_id = Guid.NewGuid(),
                entities = new List<ObjectEntity>
                {
                    new ObjectEntity
                    {
                        id = Guid.NewGuid(),
                        Properties = new List<PropertiesEntity>
                        {
                            new PropertiesEntity
                            {
                                internal_status_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid()
                            }
                        },
                        filters = new List<FiltersEntity>
                        {
                            new FiltersEntity
                            {
                                operator_id = Guid.NewGuid(),
                                property_id = Guid.NewGuid(),
                                value = "value"
                            }
                        }

                    }
                },
                status_id = Guid.NewGuid()
            };

            _mockRepo.Setup(x => x.GetByCodeAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()))
                .ReturnsAsync(process);

            // Act
            var result = await _service.GetByCodeAsync(processCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(processCode, result.process_code);
            _mockRepo.Verify(x => x.GetByCodeAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByTypeAsync_ShouldReturnProcesses_WhenValidTypeIdProvided()
        {
            // Arrange
            var typeId = Guid.NewGuid();

            var processes = new List<ProcessEntity>
            {
                new ProcessEntity { process_code = "P001", status_id = Guid.NewGuid() },
                new ProcessEntity { process_code = "P002", status_id = Guid.NewGuid() }
            };

            var state = new StatusEntity 
            {
                id = Guid.NewGuid(),
                
            };
            _mockRepo.Setup(x => x.GetByTypeAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()))
                .ReturnsAsync(processes);
            _mockStatusService.Setup(x => x.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync(state);

            // Act
            var result = await _service.GetByTypeAsync(typeId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("P001", result.First().process_code);
            _mockRepo.Verify(x => x.GetByTypeAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedAsync_ShouldReturnProcesses_WhenValidPaginationModelProvided()
        {
            // Arrange
            var paginatedModel = new PaginatedModel
            {
                First = 0,
                Rows = 2,
                Sort_field = "",
                Sort_order = SortOrdering.Descending,
                Search = "",
                activeOnly = true
            };

            var processes = new List<ProcessResponseModel>
        {
            new ProcessResponseModel { process_code = "P001" },
            new ProcessResponseModel { process_code = "P002" }
        };

            _mockRepo.Setup(x => x.GetAllAsync(It.IsAny<ISpecification<ProcessEntity>>()))
                .ReturnsAsync(processes);

            // Act
            var result = await _service.GetAllPaginatedAsync(paginatedModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(x => x.GetAllAsync(It.IsAny<ISpecification<ProcessEntity>>()), Times.Once);
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
                Search = "",
                activeOnly = true
            };

            _mockRepo.Setup(x => x.GetTotalRows(It.IsAny<ISpecification<ProcessEntity>>()))
                .ReturnsAsync(count);

            // Act
            var result = await _service.GetTotalRowsAsync(paginatedModel);

            // Assert
            Assert.Equal(count, result);
            _mockRepo.Verify(x => x.GetTotalRows(It.IsAny<ISpecification<ProcessEntity>>()), Times.Once);
        }

        [Fact]
        public async Task EnsureCodeIsUnique_ShouldThrowException_WhenCodeIsNotUnique()
        {
            // Arrange
            var statusId = Guid.NewGuid(); // ID de estado existente
            var processCode = "P001";
            var existingProcess = new ProcessEntity
            {
                status_id = statusId,
                process_code = processCode
            };

            // Simulamos que el estado existe
            _mockStatusService.Setup(repo => repo.GetByIdAsync(statusId)).ReturnsAsync(new StatusEntity());

            _mockRepo.Setup(x => x.GetByCodeAsync(It.IsAny<Expression<Func<ProcessEntity, bool>>>()))
                .ReturnsAsync(existingProcess);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrchestratorArgumentException>(() => _service.InsertAsync(existingProcess));
            Assert.Equal((int)ResponseCode.NotFoundSuccessfully, exception.Details.Code);
            Assert.Equal(AppMessages.Domain_Response_CodeInUse, exception.Details.Description);
        }
    }
}
