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
                var repositoryEntity = await MapRepository(request.Repository.RepositoryRequest, Guid.NewGuid());
                await _repositoryService.InsertAsync(repositoryEntity);

                return new CreateRepositoryCommandResponse(
                    new RepositoryCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new RepositoryCreate
                        {
                            Id = repositoryEntity.id,
                            Code = repositoryEntity.code,
                            Port = repositoryEntity.port,
                            UserName = repositoryEntity.user,
                            Password = repositoryEntity.password,
                            DataBaseName = repositoryEntity.data_base_name,
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
                            Code = repositoryEntity.code,
                            Port = repositoryEntity.port,
                            UserName = repositoryEntity.user,
                            Password = repositoryEntity.password,
                            DataBaseName = repositoryEntity.data_base_name,
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
                            Code = repositoryById.code,
                            Port = repositoryById.port,
                            UserName = repositoryById.user,
                            Password = repositoryById.password,
                            DataBaseName = repositoryById.data_base_name,
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
                            Code = repositoryByCode.code,
                            Port = repositoryByCode.port,
                            UserName = repositoryByCode.user,
                            Password = repositoryByCode.password,
                            DataBaseName = repositoryByCode.data_base_name,
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
                                Code = c.code,
                                Port = c.port,
                                UserName = c.user,
                                Password = c.password,
                                DataBaseName = c.data_base_name,
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

        private async Task<RepositoryEntity> MapRepository(RepositoryCreateRequest request, Guid id)
        {
            var repositoryEntity = new RepositoryEntity()
            {
                id = id,
                code = await _codeConfiguratorService.GenerateCodeAsync(Modules.Repository),
                port = request.Port,
                user = request.UserName,
                password = request.Password,
                data_base_name = request.DataBaseName,
                auth_type_id = request.AuthTypeId,
                status_id = request.StatusId
            };
            return repositoryEntity;
        }
    }
}
