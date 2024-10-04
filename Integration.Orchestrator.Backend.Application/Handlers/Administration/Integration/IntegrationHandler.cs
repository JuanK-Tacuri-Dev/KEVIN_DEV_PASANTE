using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Integration
{
    public class IntegrationHandler(IIntegrationService<IntegrationEntity> integrationService)
        :
        IRequestHandler<CreateIntegrationCommandRequest, CreateIntegrationCommandResponse>,
        IRequestHandler<UpdateIntegrationCommandRequest, UpdateIntegrationCommandResponse>,
        IRequestHandler<DeleteIntegrationCommandRequest, DeleteIntegrationCommandResponse>,
        IRequestHandler<GetByIdIntegrationCommandRequest, GetByIdIntegrationCommandResponse>,
        IRequestHandler<GetAllPaginatedIntegrationCommandRequest, GetAllPaginatedIntegrationCommandResponse>
    {
        public readonly IIntegrationService<IntegrationEntity> _integrationService = integrationService;

        public async Task<CreateIntegrationCommandResponse> Handle(CreateIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var integrationMap = MapIntegration(request.Integration.IntegrationRequest, Guid.NewGuid());
                await _integrationService.InsertAsync(integrationMap);

                return new CreateIntegrationCommandResponse(
                    new IntegrationCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new IntegrationCreate
                        {
                            Id = integrationMap.id,
                            Name = integrationMap.integration_name,
                            StatusId = integrationMap.status_id,
                            Observations = integrationMap.integration_observations,
                            UserId = integrationMap.user_id,
                            Process = integrationMap.process.Select(p => new ProcessRequest
                            {
                                Id = p
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

        public async Task<UpdateIntegrationCommandResponse> Handle(UpdateIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var integrationFound = await _integrationService.GetByIdAsync(request.Id);
                if (integrationFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Integration.IntegrationRequest
                            });

                var integrationMap = MapIntegration(request.Integration.IntegrationRequest, request.Id);
                await _integrationService.UpdateAsync(integrationMap);

                return new UpdateIntegrationCommandResponse(
                        new IntegrationUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new IntegrationUpdate
                            {
                                Id = integrationMap.id,
                                Name = integrationMap.integration_name,
                                StatusId = integrationMap.status_id,
                                Observations = integrationMap.integration_observations,
                                UserId = integrationMap.user_id,
                                Process = integrationMap.process.Select(p => new ProcessRequest
                                {
                                    Id = p
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

        public async Task<DeleteIntegrationCommandResponse> Handle(DeleteIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var integrationFound = await _integrationService.GetByIdAsync(request.Integration.Id);
                if (integrationFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Integration
                            });

                await _integrationService.DeleteAsync(integrationFound);

                return new DeleteIntegrationCommandResponse(
                    new IntegrationDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new IntegrationDelete
                        {
                            Id = request.Integration.Id
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

        public async Task<GetByIdIntegrationCommandResponse> Handle(GetByIdIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var integrationFound = await _integrationService.GetByIdAsync(request.Integration.Id);
                if (integrationFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                            new DetailsArgumentErrors()
                            {
                                Code = (int)ResponseCode.NotFoundSuccessfully,
                                Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                                Data = request.Integration
                            });

                return new GetByIdIntegrationCommandResponse(
                    new IntegrationGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new IntegrationGetById
                        {
                            Id = integrationFound.id,
                            Name = integrationFound.integration_name,
                            StatusId = integrationFound.status_id,
                            Observations = integrationFound.integration_observations,
                            Process = integrationFound.process.Select(i => new ProcessRequest { Id = i }).ToList(),
                            UserId = integrationFound.user_id
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

        public async Task<GetAllPaginatedIntegrationCommandResponse> Handle(GetAllPaginatedIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Integration.Adapt<PaginatedModel>();
                var rows = await _integrationService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedIntegrationCommandResponse(
                    new IntegrationGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new IntegrationGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<IntegrationGetAllPaginated>()
                        }
                    });
                }
                var integrationsFound = await _integrationService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedIntegrationCommandResponse(
                    new IntegrationGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new IntegrationGetAllRows
                        {
                            Total_rows = rows,
                            Rows = integrationsFound.Select(integration => new IntegrationGetAllPaginated
                            {
                                Id = integration.id,
                                Name = integration.integration_name,
                                Status = integration.status_id,
                                Observations = integration.integration_observations,
                                Process = integration.process.Select(i => new ProcessResponse { Id = i }).ToList(),
                                UserId = integration.user_id
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

        private IntegrationEntity MapIntegration(IntegrationCreateRequest request, Guid id)
        {
            return new IntegrationEntity()
            {
                id = id,
                integration_name = request.Name,
                status_id = request.StatusId,
                integration_observations = request.Observations,
                process = request.Process.Select(i => i.Id).ToList(),
                user_id = request.UserId
            };
        }
    }
}
