using Integration.Orchestrator.Backend.Application.Models.Administration.Operator;
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
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Operator.OperatorCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Operator
{
    public class OperatorHandler(
        IOperatorService<OperatorEntity> operatorService,
        ICodeConfiguratorService codeConfiguratorService)
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
        public readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreateOperatorCommandResponse> Handle(CreateOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorEntity = await MapOperator(request.Operator.OperatorRequest, Guid.NewGuid(), true);
                await _operatorService.InsertAsync(operatorEntity);

                return new CreateOperatorCommandResponse(
                    new OperatorCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new OperatorCreate
                        {
                            Id = operatorEntity.id,
                            Code = operatorEntity.operator_code,
                            Name = operatorEntity.operator_name,
                            TypeId = operatorEntity.type_id
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

        public async Task<UpdateOperatorCommandResponse> Handle(UpdateOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorById = await _operatorService.GetByIdAsync(request.Id);
                if (operatorById == null)
                {
                    throw new ArgumentException(AppMessages.Application_OperatorNotFound);
                }

                var operatorEntity = await MapOperator(request.Operator.OperatorRequest, request.Id);
                await _operatorService.UpdateAsync(operatorEntity);

                return new UpdateOperatorCommandResponse(
                        new OperatorUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Messages = [AppMessages.Application_RespondeUpdated],
                            Data = new OperatorUpdate
                            {
                                Id = operatorEntity.id,
                                Code = operatorEntity.operator_code,
                                Name = operatorEntity.operator_name,
                                TypeId = operatorEntity.type_id
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
                        Messages = [AppMessages.Application_RespondeDeleted],
                        Data = new OperatorDelete 
                        { 
                            Id = operatorById.id,
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
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = new OperatorGetById
                        {
                            Id = operatorById.id,
                            Name = operatorById.operator_name,
                            Code = operatorById.operator_code,
                            TypeId = operatorById.type_id
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
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = new OperatorGetByCode
                        {
                            Id = operatorByCode.id,
                            Name = operatorByCode.operator_name,
                            Code = operatorByCode.operator_code,
                            TypeId = operatorByCode.type_id
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

        public async Task<GetByTypeOperatorCommandResponse> Handle(GetByTypeOperatorCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var operatorByType = await _operatorService.GetByTypeIdAsync(request.Operator.TypeId);
                if (operatorByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_OperatorNotFound);
                }

                return new GetByTypeOperatorCommandResponse(
                    new OperatorGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = operatorByType.Select(c => new OperatorGetByType
                        {
                            Id = c.id,
                            Name = c.operator_name,
                            Code = c.operator_code,
                            TypeId = c.type_id
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
                        Description = AppMessages.Application_RespondeGetAll,
                        Data = new OperatorGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new OperatorGetAllPaginated
                            {
                                Id = c.id,
                                Name = c.operator_name,
                                Code = c.operator_code,
                                TypeId = c.type_id
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

        private async Task<OperatorEntity> MapOperator(OperatorCreateRequest request, Guid id, bool? create = null)
        {
            var operatorEntity = new OperatorEntity()
            {
                id = id,
                operator_name = request.Name,
                operator_code = create == true
                ? await _codeConfiguratorService.GenerateCodeAsync(Modules.Operator)
                : null,
                type_id = request.TypeId
            };
            return operatorEntity;
        }
    }
}
