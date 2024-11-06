﻿using Integration.Orchestrator.Backend.Application.Models.Configurador.Property;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyHandler(IPropertyService<PropertyEntity> propertyService)
        :
        IRequestHandler<CreatePropertyCommandRequest, CreatePropertyCommandResponse>,
        IRequestHandler<UpdatePropertyCommandRequest, UpdatePropertyCommandResponse>,
        IRequestHandler<DeletePropertyCommandRequest, DeletePropertyCommandResponse>,
        IRequestHandler<GetByIdPropertyCommandRequest, GetByIdPropertyCommandResponse>,
        IRequestHandler<GetByCodePropertyCommandRequest, GetByCodePropertyCommandResponse>,
        IRequestHandler<GetByTypePropertyCommandRequest, GetByTypePropertyCommandResponse>,
        IRequestHandler<GetByEntityPropertyCommandRequest, GetByEntityPropertyCommandResponse>,
        IRequestHandler<GetAllPaginatedPropertyCommandRequest, GetAllPaginatedPropertyCommandResponse>
    {
        public readonly IPropertyService<PropertyEntity> _propertyService = propertyService;

        public async Task<CreatePropertyCommandResponse> Handle(CreatePropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyMap = MapProperty(request.Property.PropertyRequest, Guid.NewGuid());
                await _propertyService.InsertAsync(propertyMap);

                return new CreatePropertyCommandResponse(
                    new PropertyCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new PropertyCreate
                        {
                            Id = propertyMap.id,
                            Code = propertyMap.property_code,
                            Name = propertyMap.property_name,
                            TypeId = propertyMap.type_id,
                            EntityId = propertyMap.entity_id,
                            StatusId = propertyMap.status_id
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                var propertyFound = await _propertyService.GetByIdAsync(request.Id);
                if (propertyFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Property.PropertyRequest
                            });

                var propertyMap = MapProperty(request.Property.PropertyRequest, request.Id);
                await _propertyService.UpdateAsync(propertyMap);
              
                return new UpdatePropertyCommandResponse(
                        new PropertyUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new PropertyUpdate
                            {
                                Id = propertyMap.id,
                                Code = propertyFound.property_code,
                                Name = propertyMap.property_name,
                                TypeId = propertyMap.type_id,
                                EntityId = propertyMap.entity_id,
                                StatusId = propertyMap.status_id
                            }
                        });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                var propertyFound = await _propertyService.GetByIdAsync(request.Property.Id);
                if (propertyFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Property
                            });

                await _propertyService.DeleteAsync(propertyFound);

                return new DeletePropertyCommandResponse(
                    new PropertyDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new PropertyDelete
                        {
                            Id = propertyFound.id
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                var propertyFound = await _propertyService.GetByIdAsync(request.Property.Id);
                if (propertyFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Property
                            });

                return new GetByIdPropertyCommandResponse(
                    new PropertyGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new PropertyGetById
                        {
                            Id = propertyFound.id,
                            Name = propertyFound.property_name,
                            Code = propertyFound.property_code,
                            TypeId = propertyFound.type_id,
                            EntityId = propertyFound.entity_id,
                            StatusId = propertyFound.status_id
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                var propertyFound = await _propertyService.GetByCodeAsync(request.Property.Code);
                if (propertyFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Property
                            });

                return new GetByCodePropertyCommandResponse(
                    new PropertyGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new PropertyGetByCode
                        {
                            Id = propertyFound.id,
                            Name = propertyFound.property_name,
                            Code = propertyFound.property_code,
                            TypeId = propertyFound.type_id,
                            EntityId = propertyFound.entity_id,
                            StatusId = propertyFound.status_id
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                var propertyFound = await _propertyService.GetByTypeIdAsync(request.Property.TypeId);
                if (propertyFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Property
                            });

                return new GetByTypePropertyCommandResponse(
                    new PropertyGetByTypeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = propertyFound.Select(property => new PropertyGetByType
                        {
                            Id = property.id,
                            Name = property.property_name,
                            Code = property.property_code,
                            TypeId = property.type_id,
                            EntityId = property.entity_id,
                            StatusId = property.status_id
                        }).ToList()
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        public async Task<GetByEntityPropertyCommandResponse> Handle(GetByEntityPropertyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var propertyFound = await _propertyService.GetByEntityIdAsync(request.Property.EntityId);
                if (propertyFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Property
                            });

                return new GetByEntityPropertyCommandResponse(
                    new PropertyGetByEntityResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = propertyFound.Select(property => new PropertyGetByEntity
                        {
                            Id = property.id,
                            Name = property.property_name,
                            Code = property.property_code,
                            TypeId = property.type_id,
                            EntityId = property.entity_id,
                            StatusId = property.status_id
                        }).ToList()
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
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
                    return new GetAllPaginatedPropertyCommandResponse(
                    new PropertyGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new PropertyGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<PropertyGetAllPaginated>()
                        }
                    });
                }
                var propertiesFound = await _propertyService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedPropertyCommandResponse(
                    new PropertyGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new PropertyGetAllRows
                        {
                            Total_rows = rows,
                            Rows = propertiesFound.Select(property => new PropertyGetAllPaginated
                            {
                                Id = property.id,
                                Name = property.property_name,
                                Code = property.property_code,
                                TypeId = property.type_id,
                                EntityId = property.entity_id,
                                StatusId = property.status_id
                            }).ToList()
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        private PropertyEntity MapProperty(PropertyCreateRequest request, Guid id)
        {
            return new PropertyEntity()
            {
                id = id,
                property_name = request.Name?.Trim() ?? string.Empty,
                type_id = request.TypeId,
                entity_id = request.EntityId,
                status_id = request.StatusId
            };
        }
    }
}
