using System.Net;
using Integration.Orchestrator.Backend.Application.Models.Administration.Property;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Property
{
    public class PropertyHandler(IPropertyService<PropertyEntity> propertyService,
        ICodeConfiguratorService codeConfiguratorService)
        :
        IRequestHandler<CreatePropertyCommandRequest, CreatePropertyCommandResponse>,
        IRequestHandler<UpdatePropertyCommandRequest, UpdatePropertyCommandResponse>,
        IRequestHandler<DeletePropertyCommandRequest, DeletePropertyCommandResponse>,
        IRequestHandler<GetByIdPropertyCommandRequest, GetByIdPropertyCommandResponse>,
        IRequestHandler<GetByCodePropertyCommandRequest, GetByCodePropertyCommandResponse>,
        IRequestHandler<GetByTypePropertyCommandRequest, GetByTypePropertyCommandResponse>,
        IRequestHandler<GetAllPaginatedPropertyCommandRequest, GetAllPaginatedPropertyCommandResponse>
    {
        public readonly IPropertyService<PropertyEntity> _propertyService = propertyService;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreatePropertyCommandResponse> Handle(CreatePropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyEntity = await MapProperty(request.Property.PropertyRequest);
                await _propertyService.InsertAsync(propertyEntity);

                return new CreatePropertyCommandResponse(
                    new PropertyCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeCreated],
                        Data = new PropertyCreate
                        {
                            Id = propertyEntity.id,
                            Code = propertyEntity.property_code,
                            Name = propertyEntity.property_name,
                            TypeId = propertyEntity.type_id,
                            EntityId = propertyEntity.entity_id,
                            StatusId = propertyEntity.status_id
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<UpdatePropertyCommandResponse> Handle(UpdatePropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyById = await _propertyService.GetByIdAsync(request.Id);
                if (propertyById == null)
                {
                    throw new ArgumentException(AppMessages.Application_PropertyNotFound);
                }

                var propertyEntity = await MapProperty(request.Property.PropertyRequest, propertyById);
                await _propertyService.UpdateAsync(propertyEntity);

                return new UpdatePropertyCommandResponse(
                        new PropertyUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Messages = [AppMessages.Application_RespondeUpdated],
                            Data = new PropertyUpdate
                            {
                                Id = propertyEntity.id,
                                Code = propertyEntity.property_code,
                                Name = propertyEntity.property_name,
                                TypeId = propertyEntity.type_id,
                                EntityId = propertyEntity.entity_id,
                                StatusId = propertyEntity.status_id
                            }
                        });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<DeletePropertyCommandResponse> Handle(DeletePropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyById = await _propertyService.GetByIdAsync(request.Property.Id);
                if (propertyById == null)
                {
                    throw new ArgumentException(AppMessages.Application_PropertyNotFound);
                }

                await _propertyService.DeleteAsync(propertyById);

                return new DeletePropertyCommandResponse(
                    new PropertyDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeDeleted],
                        Data = new PropertyDelete 
                        {
                            Id = propertyById.id
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<GetByIdPropertyCommandResponse> Handle(GetByIdPropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyById = await _propertyService.GetByIdAsync(request.Property.Id);
                if (propertyById == null)
                {
                    throw new ArgumentException(AppMessages.Application_PropertyNotFound);
                }

                return new GetByIdPropertyCommandResponse(
                    new PropertyGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = new PropertyGetById
                        {
                            Id = propertyById.id,
                            Name = propertyById.property_name,
                            Code = propertyById.property_code,
                            TypeId = propertyById.type_id,
                            EntityId = propertyById.entity_id,
                            StatusId = propertyById.status_id
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<GetByCodePropertyCommandResponse> Handle(GetByCodePropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyByCode = await _propertyService.GetByCodeAsync(request.Property.Code);
                if (propertyByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_PropertyNotFound);
                }

                return new GetByCodePropertyCommandResponse(
                    new PropertyGetByCodeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = new PropertyGetByCode
                        {
                            Id = propertyByCode.id,
                            Name = propertyByCode.property_name,
                            Code = propertyByCode.property_code,
                            TypeId = propertyByCode.type_id,
                            EntityId = propertyByCode.entity_id,
                            StatusId = propertyByCode.status_id
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<GetByTypePropertyCommandResponse> Handle(GetByTypePropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyByType = await _propertyService.GetByTypeIdAsync(request.Property.TypeId);
                if (propertyByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_PropertyNotFound);
                }

                return new GetByTypePropertyCommandResponse(
                    new PropertyGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = propertyByType.Select(c => new PropertyGetByType
                        {
                            Id = c.id,
                            Name = c.property_name,
                            Code = c.property_code,
                            TypeId = c.type_id,
                            EntityId = c.entity_id,
                            StatusId = c.status_id
                        }).ToList()
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<GetAllPaginatedPropertyCommandResponse> Handle(GetAllPaginatedPropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Property.Adapt<PaginatedModel>();
                var rows = await _propertyService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_PropertyNotFound);
                }
                var result = await _propertyService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedPropertyCommandResponse(
                    new PropertyGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_RespondeGetAll,
                        Data = new PropertyGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new PropertyGetAllPaginated
                            {
                                Id = c.id,
                                Name = c.property_name,
                                Code = c.property_code,
                                TypeId = c.type_id,
                                EntityId = c.entity_id,
                                StatusId = c.status_id
                            }).ToList()
                        }
                    });
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        private async Task<PropertyEntity> MapProperty(PropertyCreateRequest request, PropertyEntity savedProperty = null)
        {
            return new PropertyEntity()
            {
                id = savedProperty == null ? Guid.NewGuid() : savedProperty.id,
                property_name = request.Name,
                property_code = savedProperty == null
                ? await _codeConfiguratorService.GenerateCodeAsync(Modules.Property)
                : savedProperty.property_code,
                type_id = request.TypeId,
                entity_id = request.EntityId,
                status_id = request.StatusId
            };
        }
    }
}
