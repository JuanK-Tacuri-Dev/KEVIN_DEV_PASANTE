using Integration.Orchestrator.Backend.Application.Models.Administration.Server;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Server
{
    public class ServerHandler(
        IServerService<ServerEntity> serverService,
        ICodeConfiguratorService codeConfiguratorService)
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
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreateServerCommandResponse> Handle(CreateServerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serverEntity = await MapServer(request.Server.ServerRequest, Guid.NewGuid(), true);
                await _serverService.InsertAsync(serverEntity);

                return new CreateServerCommandResponse(
                    new ServerCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new ServerCreate
                        {
                            Id = serverEntity.id,
                            Code = serverEntity.code,
                            Name = serverEntity.name,
                            TypeServerId = serverEntity.type_server_id,
                            Url = serverEntity.url,
                            StatusId = serverEntity.status_id
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

                var serverEntity = await MapServer(request.Server.ServerRequest, request.Id);
                await _serverService.UpdateAsync(serverEntity);

                return new UpdateServerCommandResponse(
                        new ServerUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new ServerUpdate
                            {
                                Id = serverEntity.id,
                                Code = serverEntity.code,
                                Name = serverEntity.name,
                                TypeServerId = serverEntity.type_server_id,
                                Url = serverEntity.url,
                                StatusId = serverEntity.status_id
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
                            Code = serverById.code,
                            Name = serverById.name,
                            TypeServerId = serverById.type_server_id,
                            Url = serverById.url,
                            StatusId = serverById.status_id
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
                            Code = serverByCode.code,
                            Name = serverByCode.name,
                            TypeServerId = serverByCode.type_server_id,
                            Url = serverByCode.url,
                            StatusId = serverByCode.status_id
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
                            Code = s.code,
                            Name = s.name,
                            TypeServerId = s.type_server_id,
                            Url = s.url,
                            StatusId = s.status_id

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
                            Rows = result.Select(s => new ServerGetAllPaginated
                            {
                                Id = s.id,
                                Code = s.code,
                                Name = s.name,
                                TypeServerId = s.type_server_id,
                                Url = s.url,
                                StatusId = s.status_id

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

        private async Task<ServerEntity> MapServer(ServerCreateRequest request, Guid id, bool? create = null)
        {
            var serverEntity = new ServerEntity()
            {
                id = id,
                code = create == true
                    ? await _codeConfiguratorService.GenerateCodeAsync(Modules.Server)
                    : null,
                name = request.Name,
                type_server_id = request.TypeServerId,
                url = request.Url,
                status_id = request.StatusId
            };
            return serverEntity;
        }
    }
}
