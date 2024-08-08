using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection
{
    public class ConnectionHandler(
        IConnectionService<ConnectionEntity> connectionService,
        IModuleSequenceService moduleSequenceService)
        :
        IRequestHandler<CreateConnectionCommandRequest, CreateConnectionCommandResponse>,
        IRequestHandler<UpdateConnectionCommandRequest, UpdateConnectionCommandResponse>,
        IRequestHandler<DeleteConnectionCommandRequest, DeleteConnectionCommandResponse>,
        IRequestHandler<GetByIdConnectionCommandRequest, GetByIdConnectionCommandResponse>,
        IRequestHandler<GetByCodeConnectionCommandRequest, GetByCodeConnectionCommandResponse>,
        IRequestHandler<GetAllPaginatedConnectionCommandRequest, GetAllPaginatedConnectionCommandResponse>
    {
        private readonly IConnectionService<ConnectionEntity> _connectionService = connectionService;
        private readonly IModuleSequenceService _moduleSequenceService = moduleSequenceService;

        public async Task<CreateConnectionCommandResponse> Handle(CreateConnectionCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var connectionEntity = await MapConnection(request.Connection.ConnectionRequest, Guid.NewGuid());
                await _connectionService.InsertAsync(connectionEntity);

                return new CreateConnectionCommandResponse(
                    new ConnectionCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new ConnectionCreate
                        {
                            Id = connectionEntity.id,
                            Code = connectionEntity.code,
                            ServerId = connectionEntity.server_id,
                            AdapterId = connectionEntity.adapter_id,
                            RepositoryId = connectionEntity.repository_id,
                            Description = connectionEntity.description,
                            StatusId = connectionEntity.status_id
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
                var connectionById = await _connectionService.GetByIdAsync(request.Id);
                if (connectionById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Connection.ConnectionRequest
                        });

                var connectionEntity = await MapConnection(request.Connection.ConnectionRequest, request.Id);
                await _connectionService.UpdateAsync(connectionEntity);

                return new UpdateConnectionCommandResponse(
                        new ConnectionUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new ConnectionUpdate
                            {
                                Id = connectionEntity.id,
                                Code = connectionEntity.code,
                                ServerId = connectionEntity.server_id,
                                AdapterId = connectionEntity.adapter_id,
                                RepositoryId = connectionEntity.repository_id,
                                Description = connectionEntity.description,
                                StatusId = connectionEntity.status_id
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
                var connectionById = await _connectionService.GetByIdAsync(request.Connection.Id);
                if (connectionById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Connection
                        });

                await _connectionService.DeleteAsync(connectionById);

                return new DeleteConnectionCommandResponse(
                    new ConnectionDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new ConnectionDelete 
                        {
                            Id = connectionById.id
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
                var connectionById = await _connectionService.GetByIdAsync(request.Connection.Id);
                if (connectionById == null)
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
                            Id = connectionById.id,
                            Code = connectionById.code,
                            ServerId = connectionById.server_id,
                            AdapterId = connectionById.adapter_id,
                            RepositoryId = connectionById.repository_id,
                            Description = connectionById.description,
                            StatusId = connectionById.status_id
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
                var connectionByCode = await _connectionService.GetByCodeAsync(request.Connection.Code);
                if (connectionByCode == null)
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
                            Id = connectionByCode.id,
                            Code = connectionByCode.code,
                            ServerId = connectionByCode.server_id,
                            AdapterId = connectionByCode.adapter_id,
                            RepositoryId = connectionByCode.repository_id,
                            Description = connectionByCode.description,
                            StatusId = connectionByCode.status_id
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
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
                        });
                }
                var result = await _connectionService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedConnectionCommandResponse(
                    new ConnectionGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new ConnectionGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new ConnectionGetAllPaginated
                            {
                                Id = c.id,
                                Code = c.code,
                                ServerId = c.server_id,
                                AdapterId = c.adapter_id,
                                RepositoryId = c.repository_id,
                                Description = c.description,
                                StatusId = c.status_id
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

        private async Task<ConnectionEntity> MapConnection(ConnectionCreateRequest request, Guid id)
        {
            var connectionEntity = new ConnectionEntity()
            {
                id = id,
                code = await _moduleSequenceService.GenerateCodeAsync(Modules.Connection.ToString()),
                server_id = request.ServerId,
                adapter_id = request.AdapterId,
                repository_id = request.RepositoryId,
                description = request.Description,
                status_id = request.StatusId
            };
            return connectionEntity;
        }
    }
}
