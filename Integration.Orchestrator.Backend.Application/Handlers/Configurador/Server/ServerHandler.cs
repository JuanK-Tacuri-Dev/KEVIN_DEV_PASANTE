﻿using AutoMapper;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Repository;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Server;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Services.Configurador;
using Mapster;
using Mapster.Models;
using MediatR;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Repository.RepositoryCommands;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerHandler(IServerService<ServerEntity> serverService, IMapper mapper)
        :
        IRequestHandler<CreateServerCommandRequest, CreateServerCommandResponse>,
        IRequestHandler<UpdateServerCommandRequest, UpdateServerCommandResponse>,
        IRequestHandler<DeleteServerCommandRequest, DeleteServerCommandResponse>,
        IRequestHandler<GetByIdServerCommandRequest, GetByIdServerCommandResponse>,
        IRequestHandler<GetByCodeServerCommandRequest, GetByCodeServerCommandResponse>,
        IRequestHandler<GetByTypeServerCommandRequest, GetByTypeServerCommandResponse>,
        IRequestHandler<GetAllPaginatedServerCommandRequest, GetAllPaginatedServerCommandResponse>
    {
        private readonly IServerService<ServerEntity> _serverService = serverService;
        private readonly IMapper _mapper = mapper;

        public async Task<CreateServerCommandResponse> Handle(CreateServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverMap = MapServer(request.Server.ServerRequest, Guid.NewGuid());
                await _serverService.InsertAsync(serverMap);

                return new CreateServerCommandResponse(
                    new ServerCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new ServerCreate
                        {
                            Id = serverMap.id,
                            Code = serverMap.server_code,
                            Name = serverMap.server_name,
                            TypeServerId = serverMap.type_id,
                            Url = serverMap.server_url,
                            StatusId = serverMap.status_id
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

        public async Task<UpdateServerCommandResponse> Handle(UpdateServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverFound = await _serverService.GetByIdAsync(request.Id);
                if (serverFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Server.ServerRequest
                        });

                var serverMap = MapServer(request.Server.ServerRequest, request.Id);
                await _serverService.UpdateAsync(serverMap);

                return new UpdateServerCommandResponse(
                        new ServerUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new ServerUpdate
                            {
                                Id = serverMap.id,
                                Code = serverFound.server_code,
                                Name = serverMap.server_name,
                                TypeServerId = serverMap.type_id,
                                Url = serverMap.server_url,
                                StatusId = serverMap.status_id
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

        public async Task<DeleteServerCommandResponse> Handle(DeleteServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverFound = await _serverService.GetByIdAsync(request.Server.Id);
                if (serverFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Server
                        });

                await _serverService.DeleteAsync(serverFound);

                return new DeleteServerCommandResponse(
                    new ServerDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new ServerDelete
                        {
                            Id = serverFound.id
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

        public async Task<GetByIdServerCommandResponse> Handle(GetByIdServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverFound = await _serverService.GetByIdAsync(request.Server.Id);
                if (serverFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Server
                        });

                return new GetByIdServerCommandResponse(
                    new ServerGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new ServerGetById
                        {
                            Id = serverFound.id,
                            Code = serverFound.server_code,
                            Name = serverFound.server_name,
                            TypeServerId = serverFound.type_id,
                            Url = serverFound.server_url,
                            StatusId = serverFound.status_id
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

        public async Task<GetByCodeServerCommandResponse> Handle(GetByCodeServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverFound = await _serverService.GetByCodeAsync(request.Server.Code);
                if (serverFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Server
                        });

                return new GetByCodeServerCommandResponse(
                    new ServerGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new ServerGetByCode
                        {
                            Id = serverFound.id,
                            Code = serverFound.server_code,
                            Name = serverFound.server_name,
                            TypeServerId = serverFound.type_id,
                            Url = serverFound.server_url,
                            StatusId = serverFound.status_id
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

        public async Task<GetByTypeServerCommandResponse> Handle(GetByTypeServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverFound = await _serverService.GetByTypeAsync(request.Server.Type);
                if (serverFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Server
                        });

                return new GetByTypeServerCommandResponse(
                    new ServerGetByTypeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = serverFound.Select(server => new ServerGetByType
                        {
                            Id = server.id,
                            Code = server.server_code,
                            Name = server.server_name,
                            TypeServerId = server.type_id,
                            Url = server.server_url,
                            StatusId = server.status_id

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

        public async Task<GetAllPaginatedServerCommandResponse> Handle(GetAllPaginatedServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Server.Adapt<PaginatedModel>();
                var rows = await _serverService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedServerCommandResponse(
                    new ServerGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new ServerGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<ServerGetAllPaginated>()
                        }
                    });
                }
                var repositoriesFound = await _serverService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedServerCommandResponse(
                    new ServerGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new ServerGetAllRows
                        {
                            Total_rows = rows,
                            Rows = repositoriesFound.Select(server => new ServerGetAllPaginated
                            {
                                Id = server.id,
                                Code = server.server_code,
                                Name = server.server_name,
                                TypeServerId = server.type_id,
                                TypeServerName = server.type_name,
                                Url = server.server_url,
                                StatusId = server.status_id,
                                StatusName = server.status_name
                                
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

        private ServerEntity MapServer(ServerCreateRequest request, Guid id)
        {
            return new ServerEntity()
            {
                id = id,
                server_name = request.Name?.Trim() ?? string.Empty,
                type_id = request.TypeServerId,
                server_url = request.Url?.Trim() ?? string.Empty,
                status_id = request.StatusId
            };
        }
    }
}
