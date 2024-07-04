using Integration.Orchestrator.Backend.Application.Models.Administration.Property;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Property
{
    public class PropertyHandler(IPropertyService<PropertyEntity> propertyService)
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

        public async Task<CreatePropertyCommandResponse> Handle(CreatePropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyEntity = MapAynchronizer(request.Property.PropertyRequest, Guid.NewGuid());
                await _propertyService.InsertAsync(propertyEntity);

                return new CreatePropertyCommandResponse(
                    new PropertyCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_PropertyResponseCreated,
                        Data = new PropertyCreate()
                        {
                            Id = propertyEntity.id
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

                var propertyEntity = MapAynchronizer(request.Property.PropertyRequest, request.Id);
                await _propertyService.UpdateAsync(propertyEntity);

                return new UpdatePropertyCommandResponse(
                        new PropertyUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_PropertyResponseUpdated,
                            Data = new PropertyUpdate()
                            {
                                Id = propertyEntity.id
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
                        Description = AppMessages.Application_PropertyResponseDeleted
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
                        Description = AppMessages.Api_PropertyResponse,
                        Data = new PropertyGetById
                        {
                            Id = propertyById.id,
                            Name = propertyById.name,
                            Code = propertyById.property_code,
                            Type = propertyById.property_type,
                            EntityId = propertyById.entity_id
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
                        Description = AppMessages.Api_PropertyResponse,
                        Data = new PropertyGetByCode
                        {
                            Id = propertyByCode.id,
                            Name = propertyByCode.name,
                            Code = propertyByCode.property_code,
                            Type = propertyByCode.property_type,
                            EntityId = propertyByCode.entity_id
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
                var propertyByType = await _propertyService.GetByTypeAsync(request.Property.Type);
                if (propertyByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_PropertyNotFound);
                }

                return new GetByTypePropertyCommandResponse(
                    new PropertyGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_PropertyResponse,
                        Data = propertyByType.Select(c => new PropertyGetByType
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.property_code,
                            Type = c.property_type,
                            EntityId = c.entity_id
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
                        Description = AppMessages.Api_PropertyResponse,
                        TotalRows = rows,
                        Data = result.Select(c => new PropertyGetAllPaginated
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.property_code,
                            Type = c.property_type,
                            EntityId = c.entity_id,
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

        private PropertyEntity MapAynchronizer(PropertyCreateRequest request, Guid id)
        {
            var propertyEntity = new PropertyEntity()
            {
                id = id,
                name = request.Name,
                property_code = request.Code,
                property_type = request.Type,
                entity_id = request.EntityId
            };
            return propertyEntity;
        }
    }
}
