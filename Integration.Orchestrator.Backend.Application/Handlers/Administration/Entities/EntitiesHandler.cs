using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Entities.EntitiesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Entities
{
    public class EntitiesHandler(IEntitiesService<EntitiesEntity> entitiesService)
        :
        IRequestHandler<CreateEntitiesCommandRequest, CreateEntitiesCommandResponse>,
        IRequestHandler<GetByCodeEntitiesCommandRequest, GetByCodeEntitiesCommandResponse>,
        IRequestHandler<GetByTypeEntitiesCommandRequest, GetByTypeEntitiesCommandResponse>,
        IRequestHandler<GetAllPaginatedEntitiesCommandRequest, GetAllPaginatedEntitiesCommandResponse>
    {
        public readonly IEntitiesService<EntitiesEntity> _entitiesService = entitiesService;

        public async Task<CreateEntitiesCommandResponse> Handle(CreateEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesEntity = MapAynchronizer(request.Entities.EntitiesRequest, Guid.NewGuid());
                await _entitiesService.InsertAsync(entitiesEntity);

                return new CreateEntitiesCommandResponse(
                    new EntitiesCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_EntitiesResponseCreated,
                        Data = new EntitiesCreate()
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

        public async Task<GetByCodeEntitiesCommandResponse> Handle(GetByCodeEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesByCode = await _entitiesService.GetByCodeAsync(request.Entities.Code);
                if (entitiesByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_EntitiesNotFound);
                }

                return new GetByCodeEntitiesCommandResponse(
                    new GetByCodeEntitiesResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_EntitiesResponse,
                        Data = new GetByCodeEntities
                        {
                            Id = entitiesByCode.id,
                            Name = entitiesByCode.name,
                            Code = entitiesByCode.entity_code,
                            Type = entitiesByCode.entity_type
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

        public async Task<GetByTypeEntitiesCommandResponse> Handle(GetByTypeEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesByType = await _entitiesService.GetByTypeAsync(request.Entities.Type);
                if (entitiesByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_EntitiesNotFound);
                }

                return new GetByTypeEntitiesCommandResponse(
                    new GetByTypeEntitiesResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_EntitiesResponse,
                        Data = entitiesByType.Select(c => new GetByTypeEntities
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.entity_code,
                            Type = c.entity_type
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

        public async Task<GetAllPaginatedEntitiesCommandResponse> Handle(GetAllPaginatedEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            var model = request.Entities.Adapt<PaginatedModel>();
            var rows = await _entitiesService.GetTotalRowsAsync(model);
            if (rows == 0)
            {
                throw new ArgumentException(AppMessages.Application_EntitiesNotFound);
            }
            var result = await _entitiesService.GetAllPaginatedAsync(model);


            return new GetAllPaginatedEntitiesCommandResponse(
                new EntitiesGetAllPaginatedResponse
                {
                    Code = HttpStatusCode.OK.GetHashCode(),
                    Description = AppMessages.Api_EntitiesResponse,
                    TotalRows = rows,
                    Data = result.Select(c => new EntitiesGetAllPaginated
                    {
                        Id = c.id,
                        Name = c.name,
                        Code = c.entity_code,
                        Type = c.entity_type
                    }).ToList()
                });
        }

        private EntitiesEntity MapAynchronizer(EntitiesCreateRequest request, Guid id)
        {
            var entitiesEntity = new EntitiesEntity()
            {
                id = id,
                name = request.Name,
                entity_code = request.Code,
                entity_type = request.Type
            };
            return entitiesEntity;
        }
    }
}
