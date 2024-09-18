﻿using Integration.Orchestrator.Backend.Application.Models.Administration.Process;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Process.ProcessCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Process
{
    public class ProcessHandler(
        IProcessService<ProcessEntity> processService,
        ICodeConfiguratorService codeConfiguratorService)
        :
        IRequestHandler<CreateProcessCommandRequest, CreateProcessCommandResponse>,
        IRequestHandler<UpdateProcessCommandRequest, UpdateProcessCommandResponse>,
        IRequestHandler<DeleteProcessCommandRequest, DeleteProcessCommandResponse>,
        IRequestHandler<GetByIdProcessCommandRequest, GetByIdProcessCommandResponse>,
        IRequestHandler<GetByCodeProcessCommandRequest, GetByCodeProcessCommandResponse>,
        IRequestHandler<GetByTypeProcessCommandRequest, GetByTypeProcessCommandResponse>,
        IRequestHandler<GetAllPaginatedProcessCommandRequest, GetAllPaginatedProcessCommandResponse>
    {
        public readonly IProcessService<ProcessEntity> _processService = processService;
        public readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreateProcessCommandResponse> Handle(CreateProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processEntity = await MapProcess(request.Process.ProcessRequest, Guid.NewGuid(), true);
                await _processService.InsertAsync(processEntity);

                return new CreateProcessCommandResponse(
                    new ProcessCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new ProcessCreate
                        {
                            Id = processEntity.id,
                            Name = processEntity.process_name,
                            Description = processEntity.process_description,
                            Code = processEntity.process_code,
                            TypeId = processEntity.process_type_id,
                            ConnectionId = processEntity.connection_id,
                            StatusId = processEntity.status_id,
                            Entities = processEntity.entities.Select(e =>
                            new EntitiesResponse
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
                var processById = await _processService.GetByIdAsync(request.Id);
                if (processById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Process.ProcessRequest
                            });

                var processEntity = await MapProcess(request.Process.ProcessRequest, request.Id);
                await _processService.UpdateAsync(processEntity);

                return new UpdateProcessCommandResponse(
                        new ProcessUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new ProcessUpdate
                            {
                                Id = processEntity.id,
                                Name = processEntity.process_name,
                                Description = processEntity.process_description,
                                Code = processEntity.process_code,
                                TypeId = processEntity.process_type_id,
                                ConnectionId = processEntity.connection_id,
                                StatusId = processEntity.status_id,
                                Entities = processEntity.entities.Select(e =>
                                new EntitiesResponse
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
                var processById = await _processService.GetByIdAsync(request.Process.Id);
                if (processById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Process
                            });

                await _processService.DeleteAsync(processById);

                return new DeleteProcessCommandResponse(
                    new ProcessDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new ProcessDelete
                        {
                            Id = processById.id,
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
                var processById = await _processService.GetByIdAsync(request.Process.Id);
                if (processById == null)
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
                            Id = processById.id,
                            Name = processById.process_name,
                            Description = processById.process_description,
                            Code = processById.process_code,
                            TypeId = processById.process_type_id,
                            ConnectionId = processById.connection_id,
                            StatusId = processById.status_id,
                            Entities = processById.entities.Select(e =>
                            new EntitiesResponse
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
                var processByCode = await _processService.GetByCodeAsync(request.Process.Code);
                if (processByCode == null)
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
                            Id = processByCode.id,
                            Name = processByCode.process_name,
                            Description = processByCode.process_description,
                            Code = processByCode.process_code,
                            TypeId = processByCode.process_type_id,
                            ConnectionId = processByCode.connection_id,
                            StatusId = processByCode.status_id,
                            Entities = processByCode.entities.Select(e =>
                            new EntitiesResponse
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
                var processByType = await _processService.GetByTypeAsync(request.Process.TypeId);
                if (processByType == null)
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
                        Data = processByType.Select(p => new ProcessGetByType
                        {
                            Id = p.id,
                            Name = p.process_name,
                            Description = p.process_description,
                            Code = p.process_code,
                            TypeId = p.process_type_id,
                            ConnectionId = p.connection_id,
                            StatusId = p.status_id,
                            Entities = p.entities.Select(e =>
                            new EntitiesResponse
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
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
                        });
                }
                var result = await _processService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedProcessCommandResponse(
                    new ProcessGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new ProcessGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(p => new ProcessGetAllPaginated
                            {
                                Id = p.id,
                                Name = p.process_name,
                                Description = p.process_description,
                                Code = p.process_code,
                                TypeId = p.process_type_id,
                                ConnectionId = p.connection_id,
                                StatusId = p.status_id,
                                Entities = p.entities.Select(e =>
                                new EntitiesResponse
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

        private async Task<ProcessEntity> MapProcess(ProcessCreateRequest request, Guid id, bool? create = null)
        {
            var processEntity = new ProcessEntity()
            {
                id = id,
                process_code = create == true
                    ? await _codeConfiguratorService.GenerateCodeAsync(Prefix.Process)
                    : null,
                process_name = request.Name,
                process_description = request.Description,
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
                        value = f.Value
                    }).ToList()
                }).ToList()
            };
            return processEntity;
        }
    }
}
