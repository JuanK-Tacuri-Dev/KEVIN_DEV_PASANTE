using System.Net;
using Integration.Orchestrator.Backend.Application.Models.Administration.Repository;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository
{
    public class RepositoryHandler(IRepositoryService<RepositoryEntity> repositoryService)
        :
        IRequestHandler<CreateRepositoryCommandRequest, CreateRepositoryCommandResponse>,
        IRequestHandler<UpdateRepositoryCommandRequest, UpdateRepositoryCommandResponse>,
        IRequestHandler<DeleteRepositoryCommandRequest, DeleteRepositoryCommandResponse>,
        IRequestHandler<GetByIdRepositoryCommandRequest, GetByIdRepositoryCommandResponse>,
        IRequestHandler<GetByCodeRepositoryCommandRequest, GetByCodeRepositoryCommandResponse>,
        IRequestHandler<GetAllPaginatedRepositoryCommandRequest, GetAllPaginatedRepositoryCommandResponse>
    {
        public readonly IRepositoryService<RepositoryEntity> _repositoryService = repositoryService;

        public async Task<CreateRepositoryCommandResponse> Handle(CreateRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repositoryEntity = MapAynchronizer(request.Repository.RepositoryRequest, Guid.NewGuid());
                await _repositoryService.InsertAsync(repositoryEntity);

                return new CreateRepositoryCommandResponse(
                    new RepositoryCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeCreated],
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
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
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
                {
                    throw new ArgumentException(AppMessages.Application_RepositoryNotFound);
                }

                var repositoryEntity = MapAynchronizer(request.Repository.RepositoryRequest, request.Id);
                await _repositoryService.UpdateAsync(repositoryEntity);

                return new UpdateRepositoryCommandResponse(
                        new RepositoryUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Messages = [AppMessages.Application_RespondeUpdated],
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
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
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
                {
                    throw new ArgumentException(AppMessages.Application_RepositoryNotFound);
                }

                await _repositoryService.DeleteAsync(repositoryById);

                return new DeleteRepositoryCommandResponse(
                    new RepositoryDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeDeleted],
                        Data = new RepositoryDelete 
                        {
                            Id = repositoryById.id
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

        public async Task<GetByIdRepositoryCommandResponse> Handle(GetByIdRepositoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var repositoryById = await _repositoryService.GetByIdAsync(request.Repository.Id);
                if (repositoryById == null)
                {
                    throw new ArgumentException(AppMessages.Application_RepositoryNotFound);
                }

                return new GetByIdRepositoryCommandResponse(
                    new RepositoryGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
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
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
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
                {
                    throw new ArgumentException(AppMessages.Application_RepositoryNotFound);
                }

                return new GetByCodeRepositoryCommandResponse(
                    new RepositoryGetByCodeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
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
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
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
                    throw new ArgumentException(AppMessages.Application_RepositoryNotFound);
                }
                var result = await _repositoryService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedRepositoryCommandResponse(
                    new RepositoryGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_RespondeGetAll,
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
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }

        private RepositoryEntity MapAynchronizer(RepositoryCreateRequest request, Guid id)
        {
            var repositoryEntity = new RepositoryEntity()
            {
                id = id,
                code = request.Code,
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
