using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Entities.EntitiesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Entities
{
    public class EntitiesHandler(
        IEntitiesService<EntitiesEntity> entitiesService,
        ICodeConfiguratorService codeConfiguratorService)
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
        public readonly IEntitiesService<EntitiesEntity> _entitiesService = entitiesService;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreateEntitiesCommandResponse> Handle(CreateEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesMap = await MapEntities(request.Entities.EntitiesRequest, Guid.NewGuid(), true);
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

                var entitiesMap = await MapEntities(request.Entities.EntitiesRequest, request.Id);
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

        private async Task<EntitiesEntity> MapEntities(EntitiesCreateRequest request, Guid id, bool? create = null)
        {
            return new EntitiesEntity()
            {
                id = id,
                entity_name = request.Name,
                entity_code = create == true
                ? await _codeConfiguratorService.GenerateCodeAsync(Prefix.Entity)
                : null,
                type_id = request.TypeId,
                repository_id = request.RepositoryId,
                status_id = request.StatusId
            };
        }
    }
}
