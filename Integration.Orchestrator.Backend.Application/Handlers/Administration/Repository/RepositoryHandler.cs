using Integration.Orchestrator.Backend.Application.Models.Administration.Repository;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository
{
    public class RepositoryHandler(
        IRepositoryService<RepositoryEntity> repositoryService,
        ICodeConfiguratorService codeConfiguratorService)
        :
        IRequestHandler<CreateRepositoryCommandRequest, CreateRepositoryCommandResponse>,
        IRequestHandler<UpdateRepositoryCommandRequest, UpdateRepositoryCommandResponse>,
        IRequestHandler<DeleteRepositoryCommandRequest, DeleteRepositoryCommandResponse>,
        IRequestHandler<GetByIdRepositoryCommandRequest, GetByIdRepositoryCommandResponse>,
        IRequestHandler<GetByCodeRepositoryCommandRequest, GetByCodeRepositoryCommandResponse>,
        IRequestHandler<GetAllPaginatedRepositoryCommandRequest, GetAllPaginatedRepositoryCommandResponse>
    {
        private readonly IRepositoryService<RepositoryEntity> _repositoryService = repositoryService;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreateRepositoryCommandResponse> Handle(CreateRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repositoryEntity = await MapRepository(request.Repository.RepositoryRequest, Guid.NewGuid(), true);
                await _repositoryService.InsertAsync(repositoryEntity);

                return new CreateRepositoryCommandResponse(
                    new RepositoryCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new RepositoryCreate
                        {
                            Id = repositoryEntity.id,
                            Code = repositoryEntity.repository_code,
                            Port = repositoryEntity.repository_port,
                            UserName = repositoryEntity.repository_user,
                            Password = repositoryEntity.repository_password,
                            DatabaseName = repositoryEntity.repository_databaseName,
                            AuthTypeId = repositoryEntity.auth_type_id,
                            StatusId = repositoryEntity.status_id
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
                var repositoryById = await _repositoryService.GetByIdAsync(request.Id);
                if (repositoryById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Repository.RepositoryRequest
                        });

                var repositoryEntity = await MapRepository(request.Repository.RepositoryRequest, request.Id);
                await _repositoryService.UpdateAsync(repositoryEntity);

                return new UpdateRepositoryCommandResponse(
                    new RepositoryUpdateResponse
                    {
                        Code = (int)ResponseCode.UpdatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                        Data = new RepositoryUpdate
                        {
                            Id = repositoryEntity.id,
                            Code = repositoryById.repository_code,
                            Port = repositoryEntity.repository_port,
                            UserName = repositoryEntity.repository_user,
                            Password = repositoryEntity.repository_password,
                            DatabaseName = repositoryEntity.repository_databaseName,
                            AuthTypeId = repositoryEntity.auth_type_id,
                            StatusId = repositoryEntity.status_id
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
                var repositoryById = await _repositoryService.GetByIdAsync(request.Repository.Id);
                if (repositoryById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Repository
                        });

                await _repositoryService.DeleteAsync(repositoryById);

                return new DeleteRepositoryCommandResponse(
                    new RepositoryDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new RepositoryDelete
                        {
                            Id = repositoryById.id
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
                var repositoryById = await _repositoryService.GetByIdAsync(request.Repository.Id);
                if (repositoryById == null)
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
                            Code = repositoryById.repository_code,
                            Port = repositoryById.repository_port,
                            UserName = repositoryById.repository_user,
                            Password = repositoryById.repository_password,
                            DatabaseName = repositoryById.repository_databaseName,
                            AuthTypeId = repositoryById.auth_type_id,
                            StatusId = repositoryById.status_id
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
                var repositoryByCode = await _repositoryService.GetByCodeAsync(request.Repository.Code);
                if (repositoryByCode == null)
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
                            Id = repositoryByCode.id,
                            Code = repositoryByCode.repository_code,
                            Port = repositoryByCode.repository_port,
                            UserName = repositoryByCode.repository_user,
                            Password = repositoryByCode.repository_password,
                            DatabaseName = repositoryByCode.repository_databaseName,
                            AuthTypeId = repositoryByCode.auth_type_id,
                            StatusId = repositoryByCode.status_id
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
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
                        });
                }
                var result = await _repositoryService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedRepositoryCommandResponse(
                    new RepositoryGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new RepositoryGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new RepositoryGetAllPaginated
                            {
                                Id = c.id,
                                Code = c.repository_code,
                                Port = c.repository_port,
                                UserName = c.repository_user,
                                Password = c.repository_password,
                                DatabaseName = c.repository_databaseName,
                                AuthTypeId = c.auth_type_id,
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

        private async Task<RepositoryEntity> MapRepository(RepositoryCreateRequest request, Guid id, bool? create = null)
        {
            var repositoryEntity = new RepositoryEntity()
            {
                id = id,
                repository_code = create == true
                    ? await _codeConfiguratorService.GenerateCodeAsync(Prefix.Repository)
                    : null,
                repository_port = request.Port,
                repository_user = request.UserName,
                repository_password = request.Password,
                repository_databaseName = request.DatabaseName,
                auth_type_id = request.AuthTypeId,
                status_id = request.StatusId
            };
            return repositoryEntity;
        }
    }
}
