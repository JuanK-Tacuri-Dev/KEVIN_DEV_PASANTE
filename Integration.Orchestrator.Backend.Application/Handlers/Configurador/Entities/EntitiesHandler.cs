using Integration.Orchestrator.Backend.Application.Models.Configurador.Entities;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Services.Configurador;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Entities.EntitiesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesHandler(
        IEntitiesService<EntitiesEntity> entitiesService, 
        IPropertyService<PropertyEntity> propertyService, 
        IStatusService<StatusEntity> statusService, IRepositoryService<RepositoryEntity> repositoryService)
    #region MediateR
        :
        IRequestHandler<CreateEntitiesCommandRequest, CreateEntitiesCommandResponse>,
        IRequestHandler<UpdateEntitiesCommandRequest, UpdateEntitiesCommandResponse>,
        IRequestHandler<DeleteEntitiesCommandRequest, DeleteEntitiesCommandResponse>,
        IRequestHandler<GetByIdEntitiesCommandRequest, GetByIdEntitiesCommandResponse>,
        IRequestHandler<GetByCodeEntitiesCommandRequest, GetByCodeEntitiesCommandResponse>,
        IRequestHandler<GetByTypeEntitiesCommandRequest, GetByTypeEntitiesCommandResponse>,
        IRequestHandler<GetByRepositoryIdEntitiesCommandRequest, GetByRepositoryIdEntitiesCommandResponse>,
        IRequestHandler<GetAllPaginatedEntitiesCommandRequest, GetAllPaginatedEntitiesCommandResponse>
    {
        #endregion
        private readonly IEntitiesService<EntitiesEntity> _entitiesService = entitiesService;
        private readonly IPropertyService<PropertyEntity> _propertyService = propertyService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;
        private readonly IRepositoryService<RepositoryEntity> _repositoryService = repositoryService;

        public async Task<CreateEntitiesCommandResponse> Handle(CreateEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesMap = MapEntities(request.Entities.EntitiesRequest, Guid.NewGuid());
                await _entitiesService.InsertAsync(entitiesMap);

                return new CreateEntitiesCommandResponse(
                    new EntitiesCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new EntitiesCreate
                        {
                            Id = entitiesMap.id,
                            Name = entitiesMap.entity_name,
                            Code = entitiesMap.entity_code,
                            TypeId = entitiesMap.type_id,
                            RepositoryId = entitiesMap.repository_id,
                            StatusId = entitiesMap.status_id
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

        public async Task<UpdateEntitiesCommandResponse> Handle(UpdateEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesFound = await _entitiesService.GetByIdAsync(request.Id);
                if (entitiesFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Entities.EntitiesRequest
                        });

                var entitiesMap = MapEntities(request.Entities.EntitiesRequest, request.Id);
                var StatusIsActive = await _statusService.GetStatusIsActive(entitiesMap.status_id);
                var RelationPropertyActive = await _propertyService.GetByEntityIdAsync(entitiesMap.id, await _statusService.GetIdActiveStatus());

                if (!StatusIsActive && RelationPropertyActive!=null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                    new DetailsArgumentErrors()
                    {
                        Code = (int)ResponseCode.NotDeleteDueToRelationship,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotDeleteDueToRelationship),
                        Data = request.Entities
                    });
                }
                if (StatusIsActive)
                {
                    var repositoryFound = await _repositoryService.GetByIdAsync(entitiesMap.repository_id);

                    if (repositoryFound != null && !await _statusService.GetStatusIsActive(repositoryFound.status_id))
                    {
                        throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors
                            {
                                Code = (int)ResponseCode.NotActivatedDueToInactiveRelationship,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotActivatedDueToInactiveRelationship, "Repositorio"),
                                Data = request.Entities
                            });
                    }
                }

                await _entitiesService.UpdateAsync(entitiesMap);

                return new UpdateEntitiesCommandResponse(
                        new EntitiesUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new EntitiesUpdate
                            {
                                Id = entitiesMap.id,
                                Name = entitiesMap.entity_name,
                                Code = entitiesFound.entity_code,
                                TypeId = entitiesMap.type_id,
                                RepositoryId = entitiesMap.repository_id,
                                StatusId = entitiesMap.status_id
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

        public async Task<DeleteEntitiesCommandResponse> Handle(DeleteEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesFound = await _entitiesService.GetByIdAsync(request.Entities.Id);
                if (entitiesFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Entities
                        });

                await _entitiesService.DeleteAsync(entitiesFound);

                return new DeleteEntitiesCommandResponse(
                    new EntitiesDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new EntitiesDelete
                        {
                            Id = entitiesFound.id
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

        public async Task<GetByIdEntitiesCommandResponse> Handle(GetByIdEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesFound = await _entitiesService.GetByIdAsync(request.Entities.Id);
                if (entitiesFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Entities
                        });

                return new GetByIdEntitiesCommandResponse(
                    new EntitiesGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new EntitiesGetById
                        {
                            Id = entitiesFound.id,
                            Name = entitiesFound.entity_name,
                            Code = entitiesFound.entity_code,
                            TypeId = entitiesFound.type_id,
                            RepositoryId = entitiesFound.repository_id,
                            StatusId = entitiesFound.status_id
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

        public async Task<GetByCodeEntitiesCommandResponse> Handle(GetByCodeEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesFound = await _entitiesService.GetByCodeAsync(request.Entities.Code);
                if (entitiesFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Entities
                        });

                return new GetByCodeEntitiesCommandResponse(
                    new EntitiesGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new GetByCodeEntities
                        {
                            Id = entitiesFound.id,
                            Name = entitiesFound.entity_name,
                            Code = entitiesFound.entity_code,
                            TypeId = entitiesFound.type_id,
                            RepositoryId = entitiesFound.repository_id,
                            StatusId = entitiesFound.status_id
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

        public async Task<GetByTypeEntitiesCommandResponse> Handle(GetByTypeEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesFound = await _entitiesService.GetByTypeIdAsync(request.Entities.TypeId);
                if (entitiesFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Entities
                        });

                return new GetByTypeEntitiesCommandResponse(
                    new EntitiesGetByTypeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = entitiesFound.Select(entity => new EntitiesGetByType
                        {
                            Id = entity.id,
                            Name = entity.entity_name,
                            Code = entity.entity_code,
                            TypeId = entity.type_id,
                            RepositoryId = entity.repository_id,
                            StatusId = entity.status_id
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

        public async Task<GetByRepositoryIdEntitiesCommandResponse> Handle(GetByRepositoryIdEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesFound = await _entitiesService.GetByRepositoryIdAsync(request.Entities.RepositoryId);
                if (entitiesFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Entities
                        });

                return new GetByRepositoryIdEntitiesCommandResponse(
                    new EntitiesGetByRepositoryIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = entitiesFound.Select(entity => new EntitiesGetByRepositoryId
                        {
                            Id = entity.id,
                            Name = entity.entity_name,
                            Code = entity.entity_code,
                            TypeId = entity.type_id,
                            RepositoryId = entity.repository_id,
                            StatusId = entity.status_id
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

        public async Task<GetAllPaginatedEntitiesCommandResponse> Handle(GetAllPaginatedEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Entities.Adapt<PaginatedModel>();
                var rows = await _entitiesService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedEntitiesCommandResponse(
                    new EntitiesGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new EntitiesGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<EntitiesGetAllPaginated>()
                        }
                    });
                }
                var entitiesFound = await _entitiesService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedEntitiesCommandResponse(
                    new EntitiesGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new EntitiesGetAllRows
                        {
                            Total_rows = rows,
                            Rows = entitiesFound.Select(entity => new EntitiesGetAllPaginated
                            {
                                Id = entity.id,
                                Name = entity.entity_name,
                                Code = entity.entity_code,
                                TypeId = entity.type_id,
                                RepositoryId = entity.repository_id,
                                StatusId = entity.status_id
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

        private EntitiesEntity MapEntities(EntitiesCreateRequest request, Guid id)
        {
            return new EntitiesEntity()
            {
                id = id,
                entity_name = request.Name?.Trim() ?? string.Empty,
                type_id = request.TypeId,
                repository_id = request.RepositoryId,
                status_id = request.StatusId
            };
        }
    }
}
