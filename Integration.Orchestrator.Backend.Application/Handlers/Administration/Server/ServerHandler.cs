using Integration.Orchestrator.Backend.Application.Models.Administration.Server;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Server
{
    public class ServerHandler(IServerService<ServerEntity> serverService)
        :
        IRequestHandler<CreateServerCommandRequest, CreateServerCommandResponse>,
        IRequestHandler<UpdateServerCommandRequest, UpdateServerCommandResponse>,
        IRequestHandler<DeleteServerCommandRequest, DeleteServerCommandResponse>,
        IRequestHandler<GetByIdServerCommandRequest, GetByIdServerCommandResponse>,
        IRequestHandler<GetByCodeServerCommandRequest, GetByCodeServerCommandResponse>,
        IRequestHandler<GetByTypeServerCommandRequest, GetByTypeServerCommandResponse>,
        IRequestHandler<GetAllPaginatedServerCommandRequest, GetAllPaginatedServerCommandResponse>
    {
        public readonly IServerService<ServerEntity> _serverService = serverService;

        public async Task<CreateServerCommandResponse> Handle(CreateServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverEntity = MapServer(request.Server.ServerRequest, Guid.NewGuid());
                await _serverService.InsertAsync(serverEntity);

                return new CreateServerCommandResponse(
                    new ServerCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_ServerResponseCreated,
                        Data = new ServerCreate()
                        {
                            Id = serverEntity.id
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

        public async Task<UpdateServerCommandResponse> Handle(UpdateServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverById = await _serverService.GetByIdAsync(request.Id);
                if (serverById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ServerNotFound);
                }

                var serverEntity = MapServer(request.Server.ServerRequest, request.Id);
                await _serverService.UpdateAsync(serverEntity);

                return new UpdateServerCommandResponse(
                        new ServerUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_ServerResponseUpdated,
                            Data = new ServerUpdate()
                            {
                                Id = serverEntity.id
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

        public async Task<DeleteServerCommandResponse> Handle(DeleteServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverById = await _serverService.GetByIdAsync(request.Server.Id);
                if (serverById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ServerNotFound);
                }

                await _serverService.DeleteAsync(serverById);

                return new DeleteServerCommandResponse(
                    new ServerDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_ServerResponseDeleted
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

        public async Task<GetByIdServerCommandResponse> Handle(GetByIdServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverById = await _serverService.GetByIdAsync(request.Server.Id);
                if (serverById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ServerNotFound);
                }

                return new GetByIdServerCommandResponse(
                    new ServerGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ServerResponse,
                        Data = new ServerGetById
                        {
                            Id = serverById.id,
                            Code = serverById.server_code,
                            Name = serverById.name,
                            Type = serverById.type,
                            Url = serverById.url
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

        public async Task<GetByCodeServerCommandResponse> Handle(GetByCodeServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverByCode = await _serverService.GetByCodeAsync(request.Server.Code);
                if (serverByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_ServerNotFound);
                }

                return new GetByCodeServerCommandResponse(
                    new ServerGetByCodeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ServerResponse,
                        Data = new ServerGetByCode
                        {
                            Id = serverByCode.id,
                            Code = serverByCode.server_code,
                            Name = serverByCode.name,
                            Type = serverByCode.type,
                            Url = serverByCode.url
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

        public async Task<GetByTypeServerCommandResponse> Handle(GetByTypeServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverByType = await _serverService.GetByTypeAsync(request.Server.Type);
                if (serverByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_ServerNotFound);
                }

                return new GetByTypeServerCommandResponse(
                    new ServerGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ServerResponse,
                        Data = serverByType.Select(c => new ServerGetByType
                        {
                            Id = c.id,
                            Code = c.server_code,
                            Name = c.name,
                            Type = c.type,
                            Url = c.url
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

        public async Task<GetAllPaginatedServerCommandResponse> Handle(GetAllPaginatedServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Server.Adapt<PaginatedModel>();
                var rows = await _serverService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_ServerNotFound);
                }
                var result = await _serverService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedServerCommandResponse(
                    new ServerGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ServerResponse,
                        Data = new ServerGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new ServerGetAllPaginated
                            {
                                Id = c.id,
                                Code = c.server_code,
                                Name = c.name,
                                Type = c.type,
                                Url = c.url
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

        private ServerEntity MapServer(ServerCreateRequest request, Guid id)
        {
            var serverEntity = new ServerEntity()
            {
                id = id,
                server_code = request.Code,
                name = request.Name,
                type = request.Type,
                url = request.Url
            };
            return serverEntity;
        }
    }
}
