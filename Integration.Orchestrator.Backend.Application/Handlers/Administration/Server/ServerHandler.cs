using Integration.Orchestrator.Backend.Application.Models.Administration.Server;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Server
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
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new ServerCreate
                        {
                            Id = serverEntity.id,
                            Code = serverEntity.server_code,
                            Name = serverEntity.name,
                            Type = serverEntity.type,
                            Url = serverEntity.url
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
                var serverById = await _serverService.GetByIdAsync(request.Id);
                if (serverById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Server.ServerRequest
                        });

                var serverEntity = MapServer(request.Server.ServerRequest, request.Id);
                await _serverService.UpdateAsync(serverEntity);

                return new UpdateServerCommandResponse(
                        new ServerUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new ServerUpdate
                            {
                                Id = serverEntity.id,
                                Code = serverEntity.server_code,
                                Name = serverEntity.name,
                                Type = serverEntity.type,
                                Url = serverEntity.url
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
                var serverById = await _serverService.GetByIdAsync(request.Server.Id);
                if (serverById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Server
                        });

                await _serverService.DeleteAsync(serverById);

                return new DeleteServerCommandResponse(
                    new ServerDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new ServerDelete
                        {
                            Id = serverById.id
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
                var serverById = await _serverService.GetByIdAsync(request.Server.Id);
                if (serverById == null)
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
                            Id = serverById.id,
                            Code = serverById.server_code,
                            Name = serverById.name,
                            Type = serverById.type,
                            Url = serverById.url
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
                var serverByCode = await _serverService.GetByCodeAsync(request.Server.Code);
                if (serverByCode == null)
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
                            Id = serverByCode.id,
                            Code = serverByCode.server_code,
                            Name = serverByCode.name,
                            Type = serverByCode.type,
                            Url = serverByCode.url
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
                var serverByType = await _serverService.GetByTypeAsync(request.Server.Type);
                if (serverByType == null)
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
                        Data = serverByType.Select(s => new ServerGetByType
                        {
                            Id = s.id,
                            Code = s.server_code,
                            Name = s.name,
                            Type = s.type,
                            Url = s.url

                        } ).ToList()
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
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
                        });
                }
                var result = await _serverService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedServerCommandResponse(
                    new ServerGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
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
