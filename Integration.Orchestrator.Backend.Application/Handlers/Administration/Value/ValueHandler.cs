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
        IRequestHandler<GetByCodeValueCommandRequest, GetByCodeValueCommandResponse>,
        IRequestHandler<GetByTypeValueCommandRequest, GetByTypeValueCommandResponse>,
        IRequestHandler<GetAllPaginatedValueCommandRequest, GetAllPaginatedValueCommandResponse>
    {
        public readonly IValueService<ValueEntity> _entitiesService = entitiesService;

        public async Task<CreateValueCommandResponse> Handle(CreateValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesEntity = MapAynchronizer(request.Value.ValueRequest, Guid.NewGuid());
                await _entitiesService.InsertAsync(entitiesEntity);

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

        public async Task<GetByCodeValueCommandResponse> Handle(GetByCodeValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesByCode = await _entitiesService.GetByCodeAsync(request.Value.Code);
                if (entitiesByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_ValueNotFound);
                }

                return new GetByCodeValueCommandResponse(
                    new GetByCodeValueResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ValueResponse,
                        Data = new GetByCodeValue
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
                var entitiesByType = await _entitiesService.GetByTypeAsync(request.Value.Type);
                if (entitiesByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_ValueNotFound);
                }

                return new GetByTypeValueCommandResponse(
                    new GetByTypeValueResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ValueResponse,
                        Data = entitiesByType.Select(c => new GetByTypeValue
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
            var model = request.Value.Adapt<PaginatedModel>();
            var rows = await _entitiesService.GetTotalRowsAsync(model);
            if (rows == 0)
            {
                throw new ArgumentException(AppMessages.Application_ValueNotFound);
            }
            var result = await _entitiesService.GetAllPaginatedAsync(model);


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

        private ValueEntity MapAynchronizer(ValueCreateRequest request, Guid id)
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
