using Integration.Orchestrator.Backend.Application.Models.Administration.Repository;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Repository
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
                        Description = AppMessages.Application_RepositoryResponseCreated,
                        Data = new RepositoryCreate()
                        {
                            Id = repositoryEntity.id
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
                            Description = AppMessages.Application_RepositoryResponseUpdated,
                            Data = new RepositoryUpdate()
                            {
                                Id = repositoryEntity.id
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
                        Description = AppMessages.Application_RepositoryResponseDeleted
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
                        Description = AppMessages.Api_RepositoryResponse,
                        Data = new RepositoryGetById
                        {
                            Id = request.Repository.Id,
                            Code = repositoryById.repository_code,
                            Port = repositoryById.port,
                            User = repositoryById.user,
                            Password = repositoryById.password,
                            ServerId = repositoryById.server_id,
                            AdapterId = repositoryById.adapter_id
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
                        Description = AppMessages.Api_RepositoryResponse,
                        Data = new RepositoryGetByCode
                        {
                            Id = repositoryByCode.id,
                            Code = repositoryByCode.repository_code,
                            Port = repositoryByCode.port,
                            User = repositoryByCode.user,
                            Password = repositoryByCode.password,
                            ServerId = repositoryByCode.server_id,
                            AdapterId = repositoryByCode.adapter_id
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
                        Description = AppMessages.Api_RepositoryResponse,
                        Data = new RepositoryGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new RepositoryGetAllPaginated
                            {
                                Id = c.id,
                                Code = c.repository_code,
                                Port = c.port,
                                User = c.user,
                                Password = c.password,
                                ServerId = c.server_id,
                                AdapterId = c.adapter_id
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
                repository_code = request.Code,
                port = request.Port,
                user = request.User,
                password = request.Password,
                server_id = request.ServerId,
                adapter_id = request.AdapterId
            };
            return repositoryEntity;
        }
    }
}
