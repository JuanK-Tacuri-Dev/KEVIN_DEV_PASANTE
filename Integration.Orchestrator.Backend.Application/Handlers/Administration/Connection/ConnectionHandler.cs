using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Connection
{
    public class ConnectionHandler(IConnectionService<ConnectionEntity> connectionService)
        :
        IRequestHandler<CreateConnectionCommandRequest, CreateConnectionCommandResponse>,
        IRequestHandler<UpdateConnectionCommandRequest, UpdateConnectionCommandResponse>,
        IRequestHandler<DeleteConnectionCommandRequest, DeleteConnectionCommandResponse>,
        IRequestHandler<GetByIdConnectionCommandRequest, GetByIdConnectionCommandResponse>,
        IRequestHandler<GetByCodeConnectionCommandRequest, GetByCodeConnectionCommandResponse>,
        IRequestHandler<GetByTypeConnectionCommandRequest, GetByTypeConnectionCommandResponse>,
        IRequestHandler<GetAllPaginatedConnectionCommandRequest, GetAllPaginatedConnectionCommandResponse>
    {
        public readonly IConnectionService<ConnectionEntity> _connectionService = connectionService;

        public async Task<CreateConnectionCommandResponse> Handle(CreateConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionEntity = MapConnection(request.Connection.ConnectionRequest, Guid.NewGuid());
                await _connectionService.InsertAsync(connectionEntity);

                return new CreateConnectionCommandResponse(
                    new ConnectionCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_ConnectionResponseCreated,
                        Data = new ConnectionCreate()
                        {
                            Id = connectionEntity.id
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

        public async Task<UpdateConnectionCommandResponse> Handle(UpdateConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionById = await _connectionService.GetByIdAsync(request.Id);
                if (connectionById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ConnectionNotFound);
                }

                var connectionEntity = MapConnection(request.Connection.ConnectionRequest, request.Id);
                await _connectionService.UpdateAsync(connectionEntity);

                return new UpdateConnectionCommandResponse(
                        new ConnectionUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_ConnectionResponseUpdated,
                            Data = new ConnectionUpdate()
                            {
                                Id = connectionEntity.id
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

        public async Task<DeleteConnectionCommandResponse> Handle(DeleteConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionById = await _connectionService.GetByIdAsync(request.Connection.Id);
                if (connectionById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ConnectionNotFound);
                }

                await _connectionService.DeleteAsync(connectionById);

                return new DeleteConnectionCommandResponse(
                    new ConnectionDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_ConnectionResponseDeleted
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

        public async Task<GetByIdConnectionCommandResponse> Handle(GetByIdConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionById = await _connectionService.GetByIdAsync(request.Connection.Id);
                if (connectionById == null)
                {
                    throw new ArgumentException(AppMessages.Application_ConnectionNotFound);
                }

                return new GetByIdConnectionCommandResponse(
                    new ConnectionGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ConnectionResponse,
                        Data = new ConnectionGetById
                        {
                            Id = connectionById.id,
                            Code = connectionById.connection_code,
                            Server = connectionById.server,
                            Port = connectionById.port,
                            User = connectionById.user,
                            Password = connectionById.password,
                            Adapter = connectionById.adapter,
                            RepositoryId = connectionById.repository_id
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

        public async Task<GetByCodeConnectionCommandResponse> Handle(GetByCodeConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionByCode = await _connectionService.GetByCodeAsync(request.Connection.Code);
                if (connectionByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_ConnectionNotFound);
                }

                return new GetByCodeConnectionCommandResponse(
                    new ConnectionGetByCodeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ConnectionResponse,
                        Data = new ConnectionGetByCode
                        {
                            Id = connectionByCode.id,
                            Code = connectionByCode.connection_code,
                            Server = connectionByCode.server,
                            Port = connectionByCode.port,
                            User = connectionByCode.user,
                            Password = connectionByCode.password,
                            Adapter = connectionByCode.adapter,
                            RepositoryId = connectionByCode.repository_id,
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

        public async Task<GetByTypeConnectionCommandResponse> Handle(GetByTypeConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionByType = await _connectionService.GetByTypeAsync(request.Connection.Type);
                if (connectionByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_ConnectionNotFound);
                }

                return new GetByTypeConnectionCommandResponse(
                    new ConnectionGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ConnectionResponse,
                        Data = connectionByType.Select(c => new ConnectionGetByType
                        {
                            Id = c.id,
                            Code = c.connection_code,
                            Server = c.server,
                            Port = c.port,
                            User = c.user,
                            Password = c.password,
                            Adapter = c.adapter,
                            RepositoryId = c.repository_id
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

        public async Task<GetAllPaginatedConnectionCommandResponse> Handle(GetAllPaginatedConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Connection.Adapt<PaginatedModel>();
                var rows = await _connectionService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_ConnectionNotFound);
                }
                var result = await _connectionService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedConnectionCommandResponse(
                    new ConnectionGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ConnectionResponse,
                        TotalRows = rows,
                        Data = result.Select(c => new ConnectionGetAllPaginated
                        {
                            Id = c.id,
                            Code = c.connection_code,
                            Server = c.server,
                            Port = c.port,
                            User = c.user,
                            Password = c.password,
                            Adapter = c.adapter,
                            RepositoryId = c.repository_id,

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

        private ConnectionEntity MapConnection(ConnectionCreateRequest request, Guid id)
        {
            var connectionEntity = new ConnectionEntity()
            {
                id = id,
                connection_code = request.Code,
                server = request.Server,
                port = request.Port,
                user = request.User,
                password = request.Password,
                adapter = request.Adapter,
                repository_id = request.RepositoryId
            };
            return connectionEntity;
        }
    }
}
