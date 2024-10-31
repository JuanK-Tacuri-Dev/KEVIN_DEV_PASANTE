using AutoMapper;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Server;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

        //public async Task<GetAllPaginatedServerCommandResponse> Handle(GetAllPaginatedServerCommandRequest request, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var model = request.Server.Adapt<PaginatedModel>();
        //        var rows = await _serverService.GetTotalRowsAsync(model);
        //        if (rows == 0)
        //        {
        //            return new GetAllPaginatedServerCommandResponse( new ServerGetAllPaginatedResponse(
        //                                                                        (int)ResponseCode.NotFoundSuccessfully, 
        //                                                                        ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully), 
        //                                                                        rows 
        //                                                                        ));
        //        }

        //        Stopwatch stopwatch = Stopwatch.StartNew();
        //        var serversFound = await _serverService.GetAllPaginatedAsync(model);
        //        stopwatch.Stop();
        //        TimeSpan tiempoTranscurrido = stopwatch.Elapsed;

        //        Stopwatch stopwatch2 = Stopwatch.StartNew();
        //        var resp = new GetAllPaginatedServerCommandResponse(new ServerGetAllPaginatedResponse(
        //                                                                        (int)ResponseCode.FoundSuccessfully, 
        //                                                                        ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully), 
        //                                                                        rows,
        //                                                                        _mapper.Map<IEnumerable<ServerGetAllPaginated>>(serversFound)
        //                                                                        ));


        //        stopwatch2.Stop();
        //        TimeSpan tiempoTranscurrido2 = stopwatch2.Elapsed;

        //        return resp;
        //    }
        //    catch (OrchestratorArgumentException ex)
        //    {
        //        throw new OrchestratorArgumentException(string.Empty, ex.Details);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new OrchestratorException(ex.Message);
        //    }
        //}


        //public async Task<GetAllPaginatedServerCommandResponse> Handle(GetAllPaginatedServerCommandRequest request, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var model = request.Server.Adapt<PaginatedModel>();
        //        var rows = await _serverService.GetTotalRowsAsync(model);
        //        if (rows == 0)
        //        {
        //            return new GetAllPaginatedServerCommandResponse(new ServerGetAllPaginatedResponse(
        //                                                                        (int)ResponseCode.NotFoundSuccessfully,
        //                                                                        ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
        //                                                                        rows
        //                                                                        ));
        //        }

        //        Stopwatch stopwatch = Stopwatch.StartNew();
        //        var serversFound = await _serverService.GetAllPaginatedAsync(model);
        //        stopwatch.Stop();
        //        TimeSpan tiempoTranscurrido = stopwatch.Elapsed;

        //        Stopwatch stopwatch2 = Stopwatch.StartNew();
        //        var resp = new GetAllPaginatedServerCommandResponse(new ServerGetAllPaginatedResponse(
        //                                                                        (int)ResponseCode.FoundSuccessfully,
        //                                                                        ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
        //                                                                        rows,
        //                                                                        _mapper.Map<IEnumerable<ServerGetAllPaginated>>(serversFound)
        //                                                                        ));


        //        stopwatch2.Stop();
        //        TimeSpan tiempoTranscurrido2 = stopwatch2.Elapsed;

        //        return resp;
        //    }
        //    catch (OrchestratorArgumentException ex)
        //    {
        //        throw new OrchestratorArgumentException(string.Empty, ex.Details);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new OrchestratorException(ex.Message);
        //    }
        //}

        public async Task<GetAllPaginatedServerCommandResponse> Handle(GetAllPaginatedServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Server.Adapt<PaginatedModel>();
                var rows = await _serverService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedServerCommandResponse(new ServerGetAllPaginatedResponse(
                                                                                (int)ResponseCode.NotFoundSuccessfully,
                                                                                ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                                                                rows
                                                                                ));
                }

                Stopwatch stopwatch = Stopwatch.StartNew();
                var serversFound = await _serverService.GetAllPaginatedAsyncTest(model);
                stopwatch.Stop();
                TimeSpan tiempoTranscurrido = stopwatch.Elapsed;

                Stopwatch stopwatch2 = Stopwatch.StartNew();
                var resp = new GetAllPaginatedServerCommandResponse(new ServerGetAllPaginatedResponse(
                                                                                (int)ResponseCode.FoundSuccessfully,
                                                                                ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                                                                                rows,
                                                                                serversFound
                                                                                ));


                stopwatch2.Stop();
                TimeSpan tiempoTranscurrido2 = stopwatch2.Elapsed;

                return resp;
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
