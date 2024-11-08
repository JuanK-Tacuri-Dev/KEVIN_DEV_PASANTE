using Integration.Orchestrator.Backend.Application.Models.Configurador.Connection;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionHandler(
        IConnectionService<ConnectionEntity> connectionService,
        IProcessService<ProcessEntity> processService,
        IAdapterService<AdapterEntity> adapterService,
        IServerService<ServerEntity> serverService,
        IRepositoryService<RepositoryEntity> repositoryService,
        IStatusService<StatusEntity> statusService)
    #region MediateR
        :
        IRequestHandler<CreateConnectionCommandRequest, CreateConnectionCommandResponse>,
        IRequestHandler<UpdateConnectionCommandRequest, UpdateConnectionCommandResponse>,
        IRequestHandler<DeleteConnectionCommandRequest, DeleteConnectionCommandResponse>,
        IRequestHandler<GetByIdConnectionCommandRequest, GetByIdConnectionCommandResponse>,
        IRequestHandler<GetByCodeConnectionCommandRequest, GetByCodeConnectionCommandResponse>,
        IRequestHandler<GetAllPaginatedConnectionCommandRequest, GetAllPaginatedConnectionCommandResponse>
    {
        #endregion
        private readonly IConnectionService<ConnectionEntity> _connectionService = connectionService;
        private readonly IProcessService<ProcessEntity> _processService = processService;
        private readonly IAdapterService<AdapterEntity> _adapterService = adapterService;
        private readonly IServerService<ServerEntity> _serverService = serverService;
        private readonly IRepositoryService<RepositoryEntity> _repositoryService = repositoryService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task<CreateConnectionCommandResponse> Handle(CreateConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionMap = MapConnection(request.Connection.ConnectionRequest, Guid.NewGuid());
                await _connectionService.InsertAsync(connectionMap);

                return new CreateConnectionCommandResponse(
                    new ConnectionCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new ConnectionCreate
                        {
                            Id = connectionMap.id,
                            Code = connectionMap.connection_code,
                            ServerId = connectionMap.server_id,
                            AdapterId = connectionMap.adapter_id,
                            RepositoryId = connectionMap.repository_id,
                            Name = connectionMap.connection_name,
                            Description = connectionMap.connection_description,
                            StatusId = connectionMap.status_id
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

        public async Task<UpdateConnectionCommandResponse> Handle(UpdateConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionFound = await _connectionService.GetByIdAsync(request.Id);
                if (connectionFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Connection.ConnectionRequest
                        });

                var connectionMap = MapConnection(request.Connection.ConnectionRequest, request.Id);

                var StatusIsActive = await _statusService.GetStatusIsActive(connectionMap.status_id);
                var RelationProcessActive = await _processService.GetByConnectionIdAsync(connectionMap.id, await _statusService.GetIdActiveStatus());

                if (!StatusIsActive && RelationProcessActive != null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotDeleteDueToRelationship,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotDeleteDueToRelationship),
                            Data = request.Connection
                        });
                }
                if (StatusIsActive)
                {
                    var entityMessage = new List<string>();

                    var serverFound = await _serverService.GetByIdAsync(connectionMap.server_id);
                    if (serverFound != null && !await _statusService.GetStatusIsActive(serverFound.status_id))
                        entityMessage.Add("Server");

                    var adapterFound = await _adapterService.GetByIdAsync(connectionMap.adapter_id);
                    if (adapterFound != null && !await _statusService.GetStatusIsActive(adapterFound.status_id))
                        entityMessage.Add("Adapter");

                    var repositoryFound = await _repositoryService.GetByIdAsync(connectionMap.repository_id);
                    if (adapterFound != null && !await _statusService.GetStatusIsActive(repositoryFound.status_id))
                        entityMessage.Add("Repository");

                    if (entityMessage.Count > 0)
                    {
                        throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors
                            {
                                Code = (int)ResponseCode.NotActivatedDueToInactiveRelationship,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotActivatedDueToInactiveRelationship, string.Join(", ", entityMessage)),
                                Data = request.Connection
                            });
                    }
                }


                await _connectionService.UpdateAsync(connectionMap);

                return new UpdateConnectionCommandResponse(
                        new ConnectionUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new ConnectionUpdate
                            {
                                Id = connectionMap.id,
                                Code = connectionFound.connection_code,
                                ServerId = connectionMap.server_id,
                                AdapterId = connectionMap.adapter_id,
                                RepositoryId = connectionMap.repository_id,
                                Name = connectionMap.connection_name,
                                Description = connectionMap.connection_description,
                                StatusId = connectionMap.status_id
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

        public async Task<DeleteConnectionCommandResponse> Handle(DeleteConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionFound = await _connectionService.GetByIdAsync(request.Connection.Id);
                if (connectionFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Connection
                        });

                await _connectionService.DeleteAsync(connectionFound);

                return new DeleteConnectionCommandResponse(
                    new ConnectionDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new ConnectionDelete
                        {
                            Id = connectionFound.id
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

        public async Task<GetByIdConnectionCommandResponse> Handle(GetByIdConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionFound = await _connectionService.GetByIdAsync(request.Connection.Id);
                if (connectionFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Connection
                        });

                return new GetByIdConnectionCommandResponse(
                    new ConnectionGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new ConnectionGetById
                        {
                            Id = connectionFound.id,
                            Code = connectionFound.connection_code,
                            ServerId = connectionFound.server_id,
                            AdapterId = connectionFound.adapter_id,
                            RepositoryId = connectionFound.repository_id,
                            Name = connectionFound.connection_name,
                            Description = connectionFound.connection_description,
                            StatusId = connectionFound.status_id
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

        public async Task<GetByCodeConnectionCommandResponse> Handle(GetByCodeConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionFound = await _connectionService.GetByCodeAsync(request.Connection.Code);
                if (connectionFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Connection
                        });

                return new GetByCodeConnectionCommandResponse(
                    new ConnectionGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new ConnectionGetByCode
                        {
                            Id = connectionFound.id,
                            Code = connectionFound.connection_code,
                            ServerId = connectionFound.server_id,
                            AdapterId = connectionFound.adapter_id,
                            RepositoryId = connectionFound.repository_id,
                            Name = connectionFound.connection_name,
                            Description = connectionFound.connection_description,
                            StatusId = connectionFound.status_id
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

        public async Task<GetAllPaginatedConnectionCommandResponse> Handle(GetAllPaginatedConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Connection.Adapt<PaginatedModel>();
                var rows = await _connectionService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedConnectionCommandResponse(
                    new ConnectionGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new ConnectionGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<ConnectionGetAllPaginated>()
                        }
                    });
                }
                var connectionsFound = await _connectionService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedConnectionCommandResponse(
                    new ConnectionGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new ConnectionGetAllRows
                        {
                            Total_rows = rows,
                            Rows = connectionsFound.Select(connection => new ConnectionGetAllPaginated
                            {
                                Id = connection.id,
                                Code = connection.connection_code,
                                ServerId = connection.server_id,
                                AdapterId = connection.adapter_id,
                                RepositoryId = connection.repository_id,
                                Name = connection.connection_name,
                                Description = connection.connection_description,
                                StatusId = connection.status_id
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

        private ConnectionEntity MapConnection(ConnectionCreateRequest request, Guid id)
        {
            return new ConnectionEntity()
            {
                id = id,
                server_id = request.ServerId,
                adapter_id = request.AdapterId,
                repository_id = request.RepositoryId,
                connection_name = request.Name?.Trim() ?? string.Empty,
                connection_description = request.Description?.Trim() ?? string.Empty,
                status_id = request.StatusId
            };
        }
    }
}
