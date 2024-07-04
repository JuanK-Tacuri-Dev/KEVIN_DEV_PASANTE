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
        IRequestHandler<UpdateEntitiesCommandRequest, UpdateEntitiesCommandResponse>,
        IRequestHandler<DeleteEntitiesCommandRequest, DeleteEntitiesCommandResponse>,
        IRequestHandler<GetByIdEntitiesCommandRequest, GetByIdEntitiesCommandResponse>,
        IRequestHandler<GetByCodeEntitiesCommandRequest, GetByCodeEntitiesCommandResponse>,
        IRequestHandler<GetByTypeEntitiesCommandRequest, GetByTypeEntitiesCommandResponse>,
        IRequestHandler<GetAllPaginatedEntitiesCommandRequest, GetAllPaginatedEntitiesCommandResponse>
    {
        public readonly IEntitiesService<EntitiesEntity> _entitiesService = entitiesService;

        public async Task<CreateEntitiesCommandResponse> Handle(CreateEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesEntity = MapEntities(request.Entities.EntitiesRequest, Guid.NewGuid());
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

        public async Task<UpdateEntitiesCommandResponse> Handle(UpdateEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesById = await _entitiesService.GetByIdAsync(request.Id);
                if (entitiesById == null)
                {
                    throw new ArgumentException(AppMessages.Application_EntitiesNotFound);
                }

                var entitiesEntity = MapEntities(request.Entities.EntitiesRequest, request.Id);
                await _entitiesService.UpdateAsync(entitiesEntity);

                return new UpdateEntitiesCommandResponse(
                        new EntitiesUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_EntitiesResponseUpdated,
                            Data = new EntitiesUpdate()
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

        public async Task<DeleteEntitiesCommandResponse> Handle(DeleteEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesById = await _entitiesService.GetByIdAsync(request.Entities.Id);
                if (entitiesById == null)
                {
                    throw new ArgumentException(AppMessages.Application_EntitiesNotFound);
                }

                await _entitiesService.DeleteAsync(entitiesById);

                return new DeleteEntitiesCommandResponse(
                    new EntitiesDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_EntitiesResponseDeleted
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

        public async Task<GetByIdEntitiesCommandResponse> Handle(GetByIdEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesById = await _entitiesService.GetByIdAsync(request.Entities.Id);
                if (entitiesById == null)
                {
                    throw new ArgumentException(AppMessages.Application_EntitiesNotFound);
                }

                return new GetByIdEntitiesCommandResponse(
                    new EntitiesGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_EntitiesResponse,
                        Data = new EntitiesGetById
                        {
                            Id = entitiesById.id,
                            Name = entitiesById.name,
                            Code = entitiesById.entity_code,
                            Type = entitiesById.entity_type,
                            IdServer = entitiesById.server_id
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
                    new EntitiesGetByCodeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_EntitiesResponse,
                        Data = new GetByCodeEntities
                        {
                            Id = entitiesByCode.id,
                            Name = entitiesByCode.name,
                            Code = entitiesByCode.entity_code,
                            Type = entitiesByCode.entity_type,
                            IdServer= entitiesByCode.server_id
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
                    new EntitiesGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_EntitiesResponse,
                        Data = entitiesByType.Select(c => new EntitiesGetByType
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.entity_code,
                            Type = c.entity_type,
                            IdServer = c.server_id
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
            try
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
                            Type = c.entity_type,
                            IdServer = c.server_id
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

        private EntitiesEntity MapEntities(EntitiesCreateRequest request, Guid id)
        {
            var entitiesEntity = new EntitiesEntity()
            {
                id = id,
                name = request.Name,
                entity_code = request.Code,
                entity_type = request.Type,
                server_id = request.IdServer
            };
            return entitiesEntity;
        }
    }
}
