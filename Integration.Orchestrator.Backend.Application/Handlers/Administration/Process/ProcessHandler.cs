using Integration.Orchestrator.Backend.Application.Models.Administration.Process;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Process.ProcessCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Process
{
    public class ProcessHandler(IProcessService<ProcessEntity> processService)
        :
        IRequestHandler<CreateProcessCommandRequest, CreateProcessCommandResponse>,
        IRequestHandler<GetByCodeProcessCommandRequest, GetByCodeProcessCommandResponse>,
        IRequestHandler<GetByTypeProcessCommandRequest, GetByTypeProcessCommandResponse>,
        IRequestHandler<GetAllPaginatedProcessCommandRequest, GetAllPaginatedProcessCommandResponse>
    {
        public readonly IProcessService<ProcessEntity> _processService = processService;

        public async Task<CreateProcessCommandResponse> Handle(CreateProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processEntity = MapAynchronizer(request.Process.ProcessRequest, Guid.NewGuid());
                await _processService.InsertAsync(processEntity);

                return new CreateProcessCommandResponse(
                    new ProcessCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_ProcessResponseCreated,
                        Data = new ProcessCreate()
                        {
                            Id = processEntity.id
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

        public async Task<GetByCodeProcessCommandResponse> Handle(GetByCodeProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processByCode = await _processService.GetByCodeAsync(request.Process.Code);
                if (processByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_ProcessNotFound);
                }

                return new GetByCodeProcessCommandResponse(
                    new GetByCodeProcessResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ProcessResponse,
                        Data = new GetByCodeProcess
                        {
                            Id = processByCode.id,
                            ProcessCode = processByCode.process_code,
                            Type = processByCode.process_type,
                            ConnectionId = processByCode.connection_id,
                            Objects = processByCode.objects.Select(obj => new ObjectRequest
                            {
                                Name = obj.name,
                                Filters = obj.filters.Select(f => new FilterRequest
                                {
                                    Key = f.key,
                                    Value = f.value
                                }).ToList()
                            }).ToList()
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

        public async Task<GetByTypeProcessCommandResponse> Handle(GetByTypeProcessCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var processByType = await _processService.GetByTypeAsync(request.Process.Type);
                if (processByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_ProcessNotFound);
                }

                return new GetByTypeProcessCommandResponse(
                    new GetByTypeProcessResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_ProcessResponse,
                        Data = processByType.Select(c => new GetByTypeProcess
                        {
                            Id = c.id,
                            ProcessCode = c.process_code,
                            Type = c.process_type,
                            ConnectionId = c.connection_id,
                            Objects = c.objects.Select(obj => new ObjectRequest
                            {
                                Name = obj.name,
                                Filters = obj.filters.Select(f => new FilterRequest
                                {
                                    Key = f.key,
                                    Value = f.value
                                }).ToList()
                            }).ToList()

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

        public async Task<GetAllPaginatedProcessCommandResponse> Handle(GetAllPaginatedProcessCommandRequest request, CancellationToken cancellationToken)
        {
            var model = request.Process.Adapt<PaginatedModel>();
            var rows = await _processService.GetTotalRowsAsync(model);
            if (rows == 0)
            {
                throw new ArgumentException(AppMessages.Application_ProcessNotFound);
            }
            var result = await _processService.GetAllPaginatedAsync(model);


            return new GetAllPaginatedProcessCommandResponse(
                new ProcessGetAllPaginatedResponse
                {
                    Code = HttpStatusCode.OK.GetHashCode(),
                    Description = AppMessages.Api_ProcessResponse,
                    TotalRows = rows,
                    Data = result.Select(c => new ProcessGetAllPaginated
                    {
                        Id = c.id,
                        ProcessCode = c.process_code,
                        Type = c.process_type,
                        ConnectionId = c.connection_id,
                        Objects = c.objects.Select(obj => new ObjectRequest
                        {
                            Name = obj.name,
                            Filters = obj.filters.Select(f => new FilterRequest
                            {
                                Key = f.key,
                                Value = f.value
                            }).ToList()
                        }).ToList()
                    }).ToList()
                });
        }

        private ProcessEntity MapAynchronizer(ProcessCreateRequest request, Guid id)
        {
            var processEntity = new ProcessEntity()
            {
                id = id,
                process_code = request.ProcessCode,
                process_type = request.Type,
                connection_id = request.ConnectionId,
                objects = request.Objects
                .Select(obj => new ObjectEntity
                {
                    name = obj.Name,
                    filters = obj.Filters
                    .Select(f => new FilterEntity
                    {
                        key = f.Key,
                        value = f.Value
                    }).ToList()
                }).ToList()
            };
            return processEntity;
        }
    }
}
