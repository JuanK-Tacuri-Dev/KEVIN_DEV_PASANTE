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
        IRequestHandler<UpdateOperatorCommandRequest, UpdateOperatorCommandResponse>,
        IRequestHandler<DeleteOperatorCommandRequest, DeleteOperatorCommandResponse>,
        IRequestHandler<GetByIdOperatorCommandRequest, GetByIdOperatorCommandResponse>,
        IRequestHandler<GetByCodeOperatorCommandRequest, GetByCodeOperatorCommandResponse>,
        IRequestHandler<GetByTypeOperatorCommandRequest, GetByTypeOperatorCommandResponse>,
        IRequestHandler<GetAllPaginatedOperatorCommandRequest, GetAllPaginatedOperatorCommandResponse>
    {
        public readonly IOperatorService<OperatorEntity> _operatorService = operatorService;

        public async Task<CreateOperatorCommandResponse> Handle(CreateOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorEntity = MapOperator(request.Operator.OperatorRequest, Guid.NewGuid());
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

        public async Task<UpdateOperatorCommandResponse> Handle(UpdateOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorById = await _operatorService.GetByIdAsync(request.Id);
                if (operatorById == null)
                {
                    throw new ArgumentException(AppMessages.Application_OperatorNotFound);
                }

                var operatorEntity = MapOperator(request.Operator.OperatorRequest, request.Id);
                await _operatorService.UpdateAsync(operatorEntity);

                return new UpdateOperatorCommandResponse(
                        new OperatorUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_OperatorResponseUpdated,
                            Data = new OperatorUpdate()
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

        public async Task<DeleteOperatorCommandResponse> Handle(DeleteOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorById = await _operatorService.GetByIdAsync(request.Operator.Id);
                if (operatorById == null)
                {
                    throw new ArgumentException(AppMessages.Application_OperatorNotFound);
                }

                await _operatorService.DeleteAsync(operatorById);

                return new DeleteOperatorCommandResponse(
                    new OperatorDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_OperatorResponseDeleted
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

        public async Task<GetByIdOperatorCommandResponse> Handle(GetByIdOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorById = await _operatorService.GetByIdAsync(request.Operator.Id);
                if (operatorById == null)
                {
                    throw new ArgumentException(AppMessages.Application_OperatorNotFound);
                }

                return new GetByIdOperatorCommandResponse(
                    new OperatorGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_OperatorResponse,
                        Data = new OperatorGetById
                        {
                            Id = operatorById.id,
                            Name = operatorById.name,
                            Code = operatorById.operator_code,
                            Type = operatorById.operator_type
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
                    new OperatorGetByCodeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_OperatorResponse,
                        Data = new OperatorGetByCode
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
                    new OperatorGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_OperatorResponse,
                        Data = operatorByType.Select(c => new OperatorGetByType
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
            try
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
                        Data = new OperatorGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new OperatorGetAllPaginated
                            {
                                Id = c.id,
                                Name = c.name,
                                Code = c.operator_code,
                                Type = c.operator_type
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

        private OperatorEntity MapOperator(OperatorCreateRequest request, Guid id)
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
