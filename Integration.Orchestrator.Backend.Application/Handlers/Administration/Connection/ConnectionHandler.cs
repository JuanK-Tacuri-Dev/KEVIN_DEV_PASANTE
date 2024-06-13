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
        IRequestHandler<GetByCodeConnectionCommandRequest, GetByCodeConnectionCommandResponse>,
        IRequestHandler<GetByTypeConnectionCommandRequest, GetByTypeConnectionCommandResponse>,
        IRequestHandler<GetAllPaginatedConnectionCommandRequest, GetAllPaginatedConnectionCommandResponse>
    {
        public readonly IConnectionService<ConnectionEntity> _connectionService = connectionService;

        public async Task<CreateConnectionCommandResponse> Handle(CreateConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionEntity = MapAynchronizer(request.Connection.ConnectionRequest, Guid.NewGuid());
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
                    new GetByCodeConnectionResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ConnectionResponse,
                        Data = new GetByCodeConnection
                        {
                            Id = connectionByCode.id,
                            Code = connectionByCode.code,
                            Server = connectionByCode.server,
                            Port = connectionByCode.port,
                            User = connectionByCode.user,
                            Password = connectionByCode.password,
                            Adapter = connectionByCode.adapter
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
                    new GetByTypeConnectionResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ConnectionResponse,
                        Data = connectionByType.Select(c => new GetByTypeConnection
                        {
                            Id = c.id,
                            Code = c.code,
                            Server = c.server,
                            Port = c.port,
                            User = c.user,
                            Password = c.password,
                            Adapter = c.adapter
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
                        Code = c.code,
                        Server = c.server,
                        Port = c.port,
                        User = c.user,
                        Password = c.password,
                        Adapter = c.adapter

                    }).ToList()
                }
                );
        }

        private ConnectionEntity MapAynchronizer(ConnectionCreateRequest request, Guid id)
        {
            var connectionEntity = new ConnectionEntity()
            {
                id = id,
                code = request.Code,
                server = request.Server,
                port = request.Port,
                user = request.User,
                password = request.Password,
                adapter = request.Adapter
            };
            return connectionEntity;
        }
    }
}
