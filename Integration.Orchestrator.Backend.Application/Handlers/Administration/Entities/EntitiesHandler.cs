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
                var entitiesEntity = await MapEntities(request.Entities.EntitiesRequest, Guid.NewGuid(), true);
                await _entitiesService.InsertAsync(entitiesEntity);

                return new CreateEntitiesCommandResponse(
                    new EntitiesCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new EntitiesCreate
                        {
                            Id = entitiesEntity.id,
                            Name = entitiesEntity.entity_name,
                            Code = entitiesEntity.entity_code,
                            TypeId = entitiesEntity.type_id,
                            RepositoryId = entitiesEntity.repository_id,
                            StatusId = entitiesEntity.status_id
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
                var entitiesById = await _entitiesService.GetByIdAsync(request.Id);
                if (entitiesById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Entities.EntitiesRequest
                        });

                var entitiesEntity = await MapEntities(request.Entities.EntitiesRequest, request.Id);
                await _entitiesService.UpdateAsync(entitiesEntity);

                return new UpdateEntitiesCommandResponse(
                        new EntitiesUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new EntitiesUpdate
                            {
                                Id = entitiesEntity.id,
                                Name = entitiesEntity.entity_name,
                                Code = entitiesEntity.entity_code,
                                TypeId = entitiesEntity.type_id,
                                RepositoryId = entitiesEntity.repository_id,
                                StatusId = entitiesEntity.status_id
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
                var entitiesById = await _entitiesService.GetByIdAsync(request.Entities.Id);
                if (entitiesById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Entities
                        });

                await _entitiesService.DeleteAsync(entitiesById);

                return new DeleteEntitiesCommandResponse(
                    new EntitiesDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new EntitiesDelete 
                        {
                            Id = entitiesById.id
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
                var entitiesById = await _entitiesService.GetByIdAsync(request.Entities.Id);
                if (entitiesById == null)
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
                            Id = entitiesById.id,
                            Name = entitiesById.entity_name,
                            Code = entitiesById.entity_code,
                            TypeId = entitiesById.type_id,
                            RepositoryId = entitiesById.repository_id,
                            StatusId = entitiesById.status_id
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
                var entitiesByCode = await _entitiesService.GetByCodeAsync(request.Entities.Code);
                if (entitiesByCode == null)
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
                            Id = entitiesByCode.id,
                            Name = entitiesByCode.entity_name,
                            Code = entitiesByCode.entity_code,
                            TypeId = entitiesByCode.type_id,
                            RepositoryId = entitiesByCode.repository_id,
                            StatusId = entitiesByCode.status_id
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
                var entitiesByType = await _entitiesService.GetByTypeIdAsync(request.Entities.TypeId);
                if (entitiesByType == null)
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
                        Data = entitiesByType.Select(c => new EntitiesGetByType
                        {
                            Id = c.id,
                            Name = c.entity_name,
                            Code = c.entity_code,
                            TypeId = c.type_id,
                            RepositoryId = c.repository_id,
                            StatusId = c.status_id
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
                var entities = await _entitiesService.GetByRepositoryIdAsync(request.Entities.RepositoryId);
                if (entities == null)
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
                        Data = entities.Select(c => new EntitiesGetByRepositoryId
                        {
                            Id = c.id,
                            Name = c.entity_name,
                            Code = c.entity_code,
                            TypeId = c.type_id,
                            RepositoryId = c.repository_id,
                            StatusId = c.status_id
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
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
                        });

                var result = await _entitiesService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedEntitiesCommandResponse(
                    new EntitiesGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new EntitiesGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new EntitiesGetAllPaginated
                            {
                                Id = c.id,
                                Name = c.entity_name,
                                Code = c.entity_code,
                                TypeId = c.type_id,
                                RepositoryId = c.repository_id,
                                StatusId = c.status_id
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
            var entitiesEntity = new EntitiesEntity()
            {
                id = id,
                entity_name = request.Name,
                entity_code = create == true
                    ? await _codeConfiguratorService.GenerateCodeAsync(Modules.Entity)
                    : null,
                type_id = request.TypeId,
                repository_id = request.RepositoryId,
                status_id = request.StatusId
            };
            return entitiesEntity;
        }
    }
}
