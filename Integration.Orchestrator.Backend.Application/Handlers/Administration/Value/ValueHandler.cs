using Integration.Orchestrator.Backend.Application.Models.Administration.Value;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Value.ValueCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Value
{
    public class ValueHandler(IValueService<ValueEntity> entitiesService)
        :
        IRequestHandler<CreateValueCommandRequest, CreateValueCommandResponse>,
        IRequestHandler<UpdateValueCommandRequest, UpdateValueCommandResponse>,
        IRequestHandler<DeleteValueCommandRequest, DeleteValueCommandResponse>,
        IRequestHandler<GetByIdValueCommandRequest, GetByIdValueCommandResponse>,
        IRequestHandler<GetByCodeValueCommandRequest, GetByCodeValueCommandResponse>,
        IRequestHandler<GetByTypeValueCommandRequest, GetByTypeValueCommandResponse>,
        IRequestHandler<GetAllPaginatedValueCommandRequest, GetAllPaginatedValueCommandResponse>
    {
        public readonly IValueService<ValueEntity> _valueService = entitiesService;

        public async Task<CreateValueCommandResponse> Handle(CreateValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesEntity = MapValue(request.Value.ValueRequest, Guid.NewGuid());
                await _valueService.InsertAsync(entitiesEntity);

                return new CreateValueCommandResponse(
                    new ValueCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_ValueResponseCreated,
                        Data = new ValueCreate()
                        {
                            Id = entitiesEntity.id
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

        public async Task<UpdateValueCommandResponse> Handle(UpdateValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var valueById = await _valueService.GetByIdAsync(request.Id);
                if (valueById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ValueNotFound);
                }

                var valueEntity = MapValue(request.Value.ValueRequest, request.Id);
                await _valueService.UpdateAsync(valueEntity);

                return new UpdateValueCommandResponse(
                        new ValueUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_ValueResponseUpdated,
                            Data = new ValueUpdate()
                            {
                                Id = valueEntity.id
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

        public async Task<DeleteValueCommandResponse> Handle(DeleteValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var valueById = await _valueService.GetByIdAsync(request.Value.Id);
                if (valueById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ValueNotFound);
                }

                await _valueService.DeleteAsync(valueById);

                return new DeleteValueCommandResponse(
                    new ValueDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_ValueResponseDeleted
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

        public async Task<GetByIdValueCommandResponse> Handle(GetByIdValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var valueById = await _valueService.GetByIdAsync(request.Value.Id);
                if (valueById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ValueNotFound);
                }

                return new GetByIdValueCommandResponse(
                    new ValueGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ValueResponse,
                        Data = new ValueGetById
                        {
                            Id = valueById.id,
                            Name = valueById.name,
                            Code = valueById.value_code,
                            Type = valueById.value_type
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

        public async Task<GetByCodeValueCommandResponse> Handle(GetByCodeValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesByCode = await _valueService.GetByCodeAsync(request.Value.Code);
                if (entitiesByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_ValueNotFound);
                }

                return new GetByCodeValueCommandResponse(
                    new ValueGetByCodeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ValueResponse,
                        Data = new ValueGetByCode
                        {
                            Id = entitiesByCode.id,
                            Name = entitiesByCode.name,
                            Code = entitiesByCode.value_code,
                            Type = entitiesByCode.value_type
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

        public async Task<GetByTypeValueCommandResponse> Handle(GetByTypeValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesByType = await _valueService.GetByTypeAsync(request.Value.Type);
                if (entitiesByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_ValueNotFound);
                }

                return new GetByTypeValueCommandResponse(
                    new ValueGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ValueResponse,
                        Data = entitiesByType.Select(c => new ValueGetByType
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.value_code,
                            Type = c.value_type
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

        public async Task<GetAllPaginatedValueCommandResponse> Handle(GetAllPaginatedValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Value.Adapt<PaginatedModel>();
                var rows = await _valueService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_ValueNotFound);
                }
                var result = await _valueService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedValueCommandResponse(
                    new ValueGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ValueResponse,
                        TotalRows = rows,
                        Data = result.Select(c => new ValueGetAllPaginated
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.value_code,
                            Type = c.value_type
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

        private ValueEntity MapValue(ValueCreateRequest request, Guid id)
        {
            var entitiesEntity = new ValueEntity()
            {
                id = id,
                name = request.Name,
                value_code = request.Code,
                value_type = request.Type
            };
            return entitiesEntity;
        }
    }
}
