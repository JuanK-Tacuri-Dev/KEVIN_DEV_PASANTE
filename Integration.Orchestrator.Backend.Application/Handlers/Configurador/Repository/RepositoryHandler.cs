using Integration.Orchestrator.Backend.Application.Models.Configurador.Repository;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryHandler(
        IRepositoryService<RepositoryEntity> repositoryService, 
        IConnectionService<ConnectionEntity> connectionService, 
        IStatusService<StatusEntity> statusService, 
        IEntitiesService<EntitiesEntity> entitiesService)
        #region MediateR
        :
        IRequestHandler<CreateRepositoryCommandRequest, CreateRepositoryCommandResponse>,
        IRequestHandler<UpdateRepositoryCommandRequest, UpdateRepositoryCommandResponse>,
        IRequestHandler<DeleteRepositoryCommandRequest, DeleteRepositoryCommandResponse>,
        IRequestHandler<GetByIdRepositoryCommandRequest, GetByIdRepositoryCommandResponse>,
        IRequestHandler<GetByCodeRepositoryCommandRequest, GetByCodeRepositoryCommandResponse>,
        IRequestHandler<GetAllPaginatedRepositoryCommandRequest, GetAllPaginatedRepositoryCommandResponse>
    {
        #endregion
        private readonly IRepositoryService<RepositoryEntity> _repositoryService = repositoryService;
        private readonly IConnectionService<ConnectionEntity> _connectionService = connectionService;
        public readonly IStatusService<StatusEntity> _statusService = statusService;
        public readonly IEntitiesService<EntitiesEntity> _entitiesService = entitiesService;

        public async Task<CreateRepositoryCommandResponse> Handle(CreateRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repositoryMap = MapRepository(request.Repository.RepositoryRequest, Guid.NewGuid());
                await _repositoryService.InsertAsync(repositoryMap);

                return new CreateRepositoryCommandResponse(
                    new RepositoryCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new RepositoryCreate
                        {
                            Id = repositoryMap.id,
                            Code = repositoryMap.repository_code,
                            Port = repositoryMap.repository_port,
                            UserName = repositoryMap.repository_userName,
                            Password = repositoryMap.repository_password,
                            DatabaseName = repositoryMap.repository_databaseName,
                            AuthTypeId = repositoryMap.auth_type_id,
                            StatusId = repositoryMap.status_id
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

        public async Task<UpdateRepositoryCommandResponse> Handle(UpdateRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repositoryFound = await _repositoryService.GetByIdAsync(request.Id);
                if (repositoryFound == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Repository.RepositoryRequest
                        });
                }

                var repositoryMap = MapRepository(request.Repository.RepositoryRequest, request.Id);
                var idstatusActive = await _statusService.GetIdActiveStatus();
                var statusIsActive = await _statusService.GetStatusIsActive(repositoryMap.status_id);

                var relationConnectionActive = await _connectionService.GetByRepositoryIdAsync(repositoryMap.id, idstatusActive);
                

                var relationEntitys = await _entitiesService.GetByRepositoryIdAsync(repositoryMap.id, await _statusService.GetIdActiveStatus());

                if (!statusIsActive && (relationConnectionActive != null || (relationEntitys!= null && relationEntitys.Count()>0)))
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotDeleteDueToRelationship,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotDeleteDueToRelationship),
                            Data = request.Repository
                        });
                }

                await _repositoryService.UpdateAsync(repositoryMap);

                return new UpdateRepositoryCommandResponse(
                    new RepositoryUpdateResponse
                    {
                        Code = (int)ResponseCode.UpdatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                        Data = new RepositoryUpdate
                        {
                            Id = repositoryMap.id,
                            Code = repositoryFound.repository_code,
                            Port = repositoryMap.repository_port,
                            UserName = repositoryMap.repository_userName,
                            Password = repositoryMap.repository_password,
                            DatabaseName = repositoryMap.repository_databaseName,
                            AuthTypeId = repositoryMap.auth_type_id,
                            StatusId = repositoryMap.status_id
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
        public async Task<DeleteRepositoryCommandResponse> Handle(DeleteRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repositoryFound = await _repositoryService.GetByIdAsync(request.Repository.Id);
                if (repositoryFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Repository
                        });

                await _repositoryService.DeleteAsync(repositoryFound);

                return new DeleteRepositoryCommandResponse(
                    new RepositoryDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new RepositoryDelete
                        {
                            Id = repositoryFound.id
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

        public async Task<GetByIdRepositoryCommandResponse> Handle(GetByIdRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repositoryFound = await _repositoryService.GetByIdAsync(request.Repository.Id);
                if (repositoryFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Repository
                        });

                return new GetByIdRepositoryCommandResponse(
                    new RepositoryGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new RepositoryGetById
                        {
                            Id = request.Repository.Id,
                            Code = repositoryFound.repository_code,
                            Port = repositoryFound.repository_port,
                            UserName = repositoryFound.repository_userName,
                            Password = repositoryFound.repository_password,
                            DatabaseName = repositoryFound.repository_databaseName,
                            AuthTypeId = repositoryFound.auth_type_id,
                            StatusId = repositoryFound.status_id
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

        public async Task<GetByCodeRepositoryCommandResponse> Handle(GetByCodeRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repositoryFound = await _repositoryService.GetByCodeAsync(request.Repository.Code);
                if (repositoryFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Repository
                        });

                return new GetByCodeRepositoryCommandResponse(
                    new RepositoryGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new RepositoryGetByCode
                        {
                            Id = repositoryFound.id,
                            Code = repositoryFound.repository_code,
                            Port = repositoryFound.repository_port,
                            UserName = repositoryFound.repository_userName,
                            Password = repositoryFound.repository_password,
                            DatabaseName = repositoryFound.repository_databaseName,
                            AuthTypeId = repositoryFound.auth_type_id,
                            StatusId = repositoryFound.status_id
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

        public async Task<GetAllPaginatedRepositoryCommandResponse> Handle(GetAllPaginatedRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Repository.Adapt<PaginatedModel>();
                var rows = await _repositoryService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedRepositoryCommandResponse(
                    new RepositoryGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new RepositoryGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<RepositoryGetAllPaginated>()
                        }
                    });
                }
                var repositoriesFound = await _repositoryService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedRepositoryCommandResponse(
                    new RepositoryGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new RepositoryGetAllRows
                        {
                            Total_rows = rows,
                            Rows = repositoriesFound.Select(repository => new RepositoryGetAllPaginated
                            {
                                Id = repository.id,
                                Code = repository.repository_code,
                                Port = repository.repository_port,
                                UserName = repository.repository_userName,
                                Password = repository.repository_password,
                                DatabaseName = repository.repository_databaseName,
                                AuthTypeId = repository.auth_type_id,
                                StatusId = repository.status_id
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

        private RepositoryEntity MapRepository(RepositoryCreateRequest request, Guid id, bool? create = null)
        {
            return new RepositoryEntity()
            {
                id = id,
                repository_port = request.Port,
                repository_userName = request.UserName?.Trim() ?? string.Empty,
                repository_password = request.Password?.Trim() ?? string.Empty,
                repository_databaseName = request.DatabaseName?.Trim() ?? string.Empty,
                auth_type_id = request.AuthTypeId,
                status_id = request.StatusId
            };
        }
    }
}
