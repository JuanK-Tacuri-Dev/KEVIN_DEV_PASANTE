﻿using Integration.Orchestrator.Backend.Application.Models.Configurador.Process;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Services.Configurador;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Process.ProcessCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configuradors.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessHandler(
        IProcessService<ProcessEntity> processService,
        IConnectionService<ConnectionEntity> connectionService,
        IIntegrationService<IntegrationEntity> integrationService,
        IStatusService<StatusEntity> statusService)
    #region MediateR
        :
        IRequestHandler<CreateProcessCommandRequest, CreateProcessCommandResponse>,
        IRequestHandler<UpdateProcessCommandRequest, UpdateProcessCommandResponse>,
        IRequestHandler<DeleteProcessCommandRequest, DeleteProcessCommandResponse>,
        IRequestHandler<GetByIdProcessCommandRequest, GetByIdProcessCommandResponse>,
        IRequestHandler<GetByCodeProcessCommandRequest, GetByCodeProcessCommandResponse>,
        IRequestHandler<GetByTypeProcessCommandRequest, GetByTypeProcessCommandResponse>,
        IRequestHandler<GetAllPaginatedProcessCommandRequest, GetAllPaginatedProcessCommandResponse>
    {
        #endregion
        private readonly IProcessService<ProcessEntity> _processService = processService;
        private readonly IConnectionService<ConnectionEntity> _connectionService = connectionService;
        private readonly IIntegrationService<IntegrationEntity> _integrationService = integrationService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;
        public async Task<CreateProcessCommandResponse> Handle(CreateProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processMap = MapProcess(request.Process.ProcessRequest, Guid.NewGuid());
                await _processService.InsertAsync(processMap);

                return new CreateProcessCommandResponse(
                    new ProcessCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new ProcessCreate
                        {
                            Id = processMap.id,
                            Name = processMap.process_name,
                            Description = processMap.process_description,
                            Code = processMap.process_code,
                            TypeId = processMap.process_type_id,
                            ConnectionId = processMap.connection_id,
                            StatusId = processMap.status_id,
                            Entities = processMap.entities.Select(e =>
                            new EntityResponse
                            {
                                Id = e.id,
                                Properties = e.Properties.Select(p => new PropertiesResponse
                                {
                                    Id = p.property_id,
                                    InternalStatusId = p.internal_status_id
                                }).ToList(),
                                Filters = e.filters.Select(f => new FilterResponse
                                {
                                    PropertyId = f.property_id,
                                    OperatorId = f.operator_id,
                                    Value = f.value
                                }).ToList()
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

        public async Task<UpdateProcessCommandResponse> Handle(UpdateProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processFound = await _processService.GetByIdAsync(request.Id);
                if (processFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Process.ProcessRequest
                            });

                var processMap = MapProcess(request.Process.ProcessRequest, request.Id);

                var StatusIsActive = await _statusService.GetStatusIsActiveAsync(processMap.status_id);
                var RelationIntegrationActive = await _integrationService.GetByProcessIdAsync(processMap.id, await _statusService.GetIdActiveStatusAsync());
                if (!StatusIsActive && RelationIntegrationActive != null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                       new DetailsArgumentErrors()
                       {
                           Code = (int)ResponseCode.NotDeleteDueToRelationship,
                           Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotDeleteDueToRelationship),
                           Data = request.Process
                       });

                }

                if (StatusIsActive)
                {
                    var connectionFound = await _connectionService.GetByIdAsync(processMap.connection_id);
                    if (connectionFound != null && !await _statusService.GetStatusIsActiveAsync(connectionFound.status_id))
                    {
                        throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors
                            {
                                Code = (int)ResponseCode.NotActivatedDueToInactiveRelationship,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotActivatedDueToInactiveRelationship, "Conexión"),
                                Data = request.Process
                            });
                    }

                }
                await _processService.UpdateAsync(processMap);

                return new UpdateProcessCommandResponse(
                        new ProcessUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new ProcessUpdate
                            {
                                Id = processMap.id,
                                Name = processMap.process_name,
                                Description = processMap.process_description,
                                Code = processFound.process_code,
                                TypeId = processMap.process_type_id,
                                ConnectionId = processMap.connection_id,
                                StatusId = processMap.status_id,
                                Entities = processMap.entities.Select(e =>
                                new EntityResponse
                                {
                                    Id = e.id,
                                    Properties = e.Properties.Select(p => new PropertiesResponse
                                    {
                                        Id = p.property_id,
                                        InternalStatusId = p.internal_status_id
                                    }).ToList(),
                                    Filters = e.filters.Select(f => new FilterResponse
                                    {
                                        PropertyId = f.property_id,
                                        OperatorId = f.operator_id,
                                        Value = f.value
                                    }).ToList()
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

        public async Task<DeleteProcessCommandResponse> Handle(DeleteProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processFound = await _processService.GetByIdAsync(request.Process.Id);
                if (processFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Process
                            });

                await _processService.DeleteAsync(processFound);

                return new DeleteProcessCommandResponse(
                    new ProcessDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new ProcessDelete
                        {
                            Id = processFound.id,
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

        public async Task<GetByIdProcessCommandResponse> Handle(GetByIdProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processFound = await _processService.GetByIdAsync(request.Process.Id);
                if (processFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Process
                            });

                return new GetByIdProcessCommandResponse(
                    new ProcessGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new ProcessGetById
                        {
                            Id = processFound.id,
                            Name = processFound.process_name,
                            Description = processFound.process_description,
                            Code = processFound.process_code,
                            TypeId = processFound.process_type_id,
                            ConnectionId = processFound.connection_id,
                            StatusId = processFound.status_id,
                            Entities = processFound.entities.Select(e =>
                            new EntityResponse
                            {
                                Id = e.id,
                                Properties = e.Properties.Select(p => new PropertiesResponse
                                {
                                    Id = p.property_id,
                                    InternalStatusId = p.internal_status_id
                                }).ToList(),
                                Filters = e.filters.Select(f => new FilterResponse
                                {
                                    PropertyId = f.property_id,
                                    OperatorId = f.operator_id,
                                    Value = f.value
                                }).ToList()
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

        public async Task<GetByCodeProcessCommandResponse> Handle(GetByCodeProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processFound = await _processService.GetByCodeAsync(request.Process.Code);
                if (processFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Process
                            });

                return new GetByCodeProcessCommandResponse(
                    new ProcessGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new ProcessGetByCode
                        {
                            Id = processFound.id,
                            Name = processFound.process_name,
                            Description = processFound.process_description,
                            Code = processFound.process_code,
                            TypeId = processFound.process_type_id,
                            ConnectionId = processFound.connection_id,
                            StatusId = processFound.status_id,
                            Entities = processFound.entities.Select(e =>
                            new EntityResponse
                            {
                                Id = e.id,
                                Properties = e.Properties.Select(p => new PropertiesResponse
                                {
                                    Id = p.property_id,
                                    InternalStatusId = p.internal_status_id
                                }).ToList(),
                                Filters = e.filters.Select(f => new FilterResponse
                                {
                                    PropertyId = f.property_id,
                                    OperatorId = f.operator_id,
                                    Value = f.value
                                }).ToList()
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

        public async Task<GetByTypeProcessCommandResponse> Handle(GetByTypeProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processFound = await _processService.GetByTypeAsync(request.Process.TypeId);
                if (processFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Process
                            });

                return new GetByTypeProcessCommandResponse(
                    new ProcessGetByTypeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = processFound.Select(process => new ProcessGetByType
                        {
                            Id = process.id,
                            Name = process.process_name,
                            Description = process.process_description,
                            Code = process.process_code,
                            TypeId = process.process_type_id,
                            ConnectionId = process.connection_id,
                            StatusId = process.status_id,
                            Entities = process.entities.Select(e =>
                            new EntityResponse
                            {
                                Id = e.id,
                                Properties = e.Properties.Select(p => new PropertiesResponse
                                {
                                    Id = p.property_id,
                                    InternalStatusId = p.internal_status_id
                                }).ToList(),
                                Filters = e.filters.Select(f => new FilterResponse
                                {
                                    PropertyId = f.property_id,
                                    OperatorId = f.operator_id,
                                    Value = f.value
                                }).ToList()
                            }).ToList()
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

        public async Task<GetAllPaginatedProcessCommandResponse> Handle(GetAllPaginatedProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Process.Adapt<PaginatedModel>();

                var rows = await _processService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedProcessCommandResponse(
                    new ProcessGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new ProcessGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<ProcessGetAllPaginated>()
                        }
                    });
                }
                var processesFound = await _processService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedProcessCommandResponse(
                    new ProcessGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new ProcessGetAllRows
                        {
                            Total_rows = rows,
                            Rows = processesFound.Select(process => new ProcessGetAllPaginated
                            {
                                Id = process.id,
                                Name = process.process_name,
                                Description = process.process_description,
                                Code = process.process_code,
                                TypeId = process.process_type_id,
                                ConnectionId = process.connection_id,
                                StatusId = process.status_id,
                                typeProcessName = process.typeProcessName,
                                connectionName = process.connectionName,
                                
                                Entities = process.entities.Select(e =>
                                new EntityResponse
                                {
                                    Id = e.id,
                                    Properties = e.Properties.Select(p => new PropertiesResponse
                                    {
                                        Id = p.property_id,
                                        InternalStatusId = p.internal_status_id
                                    }).ToList(),
                                    Filters = e.filters.Select(f => new FilterResponse
                                    {
                                        PropertyId = f.property_id,
                                        OperatorId = f.operator_id,
                                        Value = f.value
                                    }).ToList()
                                }).ToList()
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

        private ProcessEntity MapProcess(ProcessCreateRequest request, Guid id)
        {
            return new ProcessEntity()
            {
                id = id,
                process_name = request.Name?.Trim() ?? string.Empty,
                process_description = request.Description?.Trim() ?? string.Empty,
                process_type_id = request.TypeId,
                connection_id = request.ConnectionId,
                status_id = request.StatusId,
                entities = request.Entities.Select(obj => new ObjectEntity
                {
                    id = obj.Id,
                    Properties = obj.Properties.Select(p => new PropertiesEntity
                    {
                        property_id = p.Id,
                        internal_status_id = p.InternalStatusId
                    }).ToList(),
                    filters = obj.Filters.Select(f => new FiltersEntity
                    {
                        property_id = f.PropertyId,
                        operator_id = f.OperatorId,
                        value = f.Value?.Trim() ?? string.Empty
                    }).ToList()
                }).ToList()
            };
        }
    }
}
