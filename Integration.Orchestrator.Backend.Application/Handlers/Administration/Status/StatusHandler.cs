using Integration.Orchestrator.Backend.Application.Models.Administration.Status;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Status
{
    public class StatusHandler(IStatusService<StatusEntity> statusService)
        :
        IRequestHandler<CreateStatusCommandRequest, CreateStatusCommandResponse>,
        IRequestHandler<GetAllPaginatedStatusCommandRequest, GetAllPaginatedStatusCommandResponse>
    {
        public readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task<CreateStatusCommandResponse> Handle(CreateStatusCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var statusEntity = MapAynchronizer(request.Status.StatusRequest, Guid.NewGuid());
                await _statusService.InsertAsync(statusEntity);

                return new CreateStatusCommandResponse(
                    new StatusCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_StatusResponseCreated,
                        Data = new StatusCreate()
                        {
                            Id = statusEntity.id
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

        public async Task<GetAllPaginatedStatusCommandResponse> Handle(GetAllPaginatedStatusCommandRequest request, CancellationToken cancellationToken)
        {
            var model = request.Status.Adapt<PaginatedModel>();
            var rows = await _statusService.GetTotalRowsAsync(model);
            if (rows == 0)
            {
                throw new ArgumentException(AppMessages.Application_StatusNotFound);
            }
            var result = await _statusService.GetAllPaginatedAsync(model);


            return new GetAllPaginatedStatusCommandResponse(
                new StatusGetAllPaginatedResponse
                {
                    Code = HttpStatusCode.OK.GetHashCode(),
                    Description = AppMessages.Api_StatusResponse,
                    TotalRows = rows,
                    Data = result.Select(c => new StatusGetAllPaginated
                    {
                        Id = c.id,
                        Key = c.key,
                        Text = c.text,
                        Color = c.color
                    }).ToList()
                }
                );
        }

        private StatusEntity MapAynchronizer(StatusCreateRequest request, Guid id)
        {
            var statusEntity = new StatusEntity()
            {
                id = id,
                key = request.Key,
                text = request.Text,
                color = request.Color
            };
            return statusEntity;
        }
    }
}
