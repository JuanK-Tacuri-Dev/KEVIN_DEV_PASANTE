using Integration.Orchestrator.Backend.Application.Models.Administration.Operator;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Operator.OperatorCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Operator
{
    public class OperatorHandler(IOperatorService<OperatorEntity> operatorService)
        :
        IRequestHandler<CreateOperatorCommandRequest, CreateOperatorCommandResponse>,
        IRequestHandler<GetByCodeOperatorCommandRequest, GetByCodeOperatorCommandResponse>,
        IRequestHandler<GetByTypeOperatorCommandRequest, GetByTypeOperatorCommandResponse>,
        IRequestHandler<GetAllPaginatedOperatorCommandRequest, GetAllPaginatedOperatorCommandResponse>
    {
        public readonly IOperatorService<OperatorEntity> _operatorService = operatorService;

        public async Task<CreateOperatorCommandResponse> Handle(CreateOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorEntity = MapAynchronizer(request.Operator.OperatorRequest, Guid.NewGuid());
                await _operatorService.InsertAsync(operatorEntity);

                return new CreateOperatorCommandResponse(
                    new OperatorCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_OperatorResponseCreated,
                        Data = new OperatorCreate()
                        {
                            Id = operatorEntity.id
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

        public async Task<GetByCodeOperatorCommandResponse> Handle(GetByCodeOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorByCode = await _operatorService.GetByCodeAsync(request.Operator.Code);
                if (operatorByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_OperatorNotFound);
                }

                return new GetByCodeOperatorCommandResponse(
                    new GetByCodeOperatorResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_OperatorResponse,
                        Data = new GetByCodeOperator
                        {
                            Id = operatorByCode.id,
                            Name = operatorByCode.name,
                            Code = operatorByCode.operator_code,
                            Type = operatorByCode.operator_type
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

        public async Task<GetByTypeOperatorCommandResponse> Handle(GetByTypeOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorByType = await _operatorService.GetByTypeAsync(request.Operator.Type);
                if (operatorByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_OperatorNotFound);
                }

                return new GetByTypeOperatorCommandResponse(
                    new GetByTypeOperatorResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_OperatorResponse,
                        Data = operatorByType.Select(c => new GetByTypeOperator
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.operator_code,
                            Type = c.operator_type
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

        public async Task<GetAllPaginatedOperatorCommandResponse> Handle(GetAllPaginatedOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            var model = request.Operator.Adapt<PaginatedModel>();
            var rows = await _operatorService.GetTotalRowsAsync(model);
            if (rows == 0)
            {
                throw new ArgumentException(AppMessages.Application_OperatorNotFound);
            }
            var result = await _operatorService.GetAllPaginatedAsync(model);


            return new GetAllPaginatedOperatorCommandResponse(
                new OperatorGetAllPaginatedResponse
                {
                    Code = HttpStatusCode.OK.GetHashCode(),
                    Description = AppMessages.Api_OperatorResponse,
                    TotalRows = rows,
                    Data = result.Select(c => new OperatorGetAllPaginated
                    {
                        Id = c.id,
                        Name = c.name,
                        Code = c.operator_code,
                        Type = c.operator_type
                    }).ToList()
                });
        }

        private OperatorEntity MapAynchronizer(OperatorCreateRequest request, Guid id)
        {
            var operatorEntity = new OperatorEntity()
            {
                id = id,
                name = request.Name,
                operator_code = request.Code,
                operator_type = request.Type
            };
            return operatorEntity;
        }
    }
}
