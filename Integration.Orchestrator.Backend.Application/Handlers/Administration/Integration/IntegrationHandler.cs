using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
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
                var integrationEntity = MapIntegration(request.Integration.IntegrationRequest, Guid.NewGuid());
                await _integrationService.InsertAsync(integrationEntity);

                return new CreateIntegrationCommandResponse(
                    new IntegrationCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_IntegrationResponseCreated,
                        Data = new IntegrationCreate()
                        {
                            Id = integrationEntity.id
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

        public async Task<UpdateIntegrationCommandResponse> Handle(UpdateIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var integrationById = await _integrationService.GetByIdAsync(request.Id);
                if (integrationById == null)
                {
                    throw new ArgumentException(AppMessages.Application_IntegrationNotFound);
                }

                var integrationEntity = MapIntegration(request.Integration.IntegrationRequest, request.Id);
                await _integrationService.UpdateAsync(integrationEntity);

                return new UpdateIntegrationCommandResponse(
                        new IntegrationUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_IntegrationResponseUpdated,
                            Data = new IntegrationUpdate()
                            {
                                Id = integrationEntity.id
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

        public async Task<DeleteIntegrationCommandResponse> Handle(DeleteIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var integrationById = await _integrationService.GetByIdAsync(request.Integration.Id);
                if (integrationById == null)
                {
                    throw new ArgumentException(AppMessages.Application_IntegrationNotFound);
                }

                await _integrationService.DeleteAsync(integrationById);

                return new DeleteIntegrationCommandResponse(
                    new IntegrationDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_IntegrationResponseDeleted
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

        public async Task<GetByIdIntegrationCommandResponse> Handle(GetByIdIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var integrationById = await _integrationService.GetByIdAsync(request.Integration.Id);
                if (integrationById == null)
                {
                    throw new ArgumentException(AppMessages.Application_IntegrationNotFound);
                }

                return new GetByIdIntegrationCommandResponse(
                    new IntegrationGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_IntegrationResponse,
                        Data = new IntegrationGetById
                        {
                            Id = integrationById.id,
                            Name = integrationById.name,
                            Status = integrationById.status,
                            Observations = integrationById.observations,
                            Process = integrationById.process.Select(i => new ProcessRequest { Id = i }).ToList(),
                            UserId = integrationById.user_id
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

        public async Task<GetAllPaginatedIntegrationCommandResponse> Handle(GetAllPaginatedIntegrationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Integration.Adapt<PaginatedModel>();
                var rows = await _integrationService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_IntegrationNotFound);
                }
                var result = await _integrationService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedIntegrationCommandResponse(
                    new IntegrationGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_IntegrationResponse,
                        Data = new IntegrationGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(syn => new IntegrationGetAllPaginated
                            {
                                Id = syn.id,
                                Name = syn.name,
                                Status = syn.status,
                                Observations = syn.observations,
                                Process = syn.process.Select(i => new ProcessResponse { Id = i }).ToList(),
                                UserId = syn.user_id
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

        private IntegrationEntity MapIntegration(IntegrationCreateRequest request, Guid id)
        {
            var integrationEntity = new IntegrationEntity()
            {
                id = id,
                name = request.Name,
                status = request.Status,
                observations = request.Observations,
                process = request.Process.Select(i => i.Id).ToList(),
                user_id = request.UserId
            };
            return integrationEntity;
        }
    }
}
