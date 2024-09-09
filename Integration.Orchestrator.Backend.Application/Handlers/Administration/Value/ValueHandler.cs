using Integration.Orchestrator.Backend.Application.Models.Administration.Value;
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
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Value.ValueCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Value
{
    public class ValueHandler(
        IValueService<ValueEntity> entitiesService,
        ICodeConfiguratorService codeConfiguratorService)
        :
        IRequestHandler<CreateValueCommandRequest, CreateValueCommandResponse>,
        IRequestHandler<UpdateValueCommandRequest, UpdateValueCommandResponse>,
        IRequestHandler<DeleteValueCommandRequest, DeleteValueCommandResponse>,
        IRequestHandler<GetByIdValueCommandRequest, GetByIdValueCommandResponse>,
        IRequestHandler<GetByCodeValueCommandRequest, GetByCodeValueCommandResponse>,
        IRequestHandler<GetByTypeValueCommandRequest, GetByTypeValueCommandResponse>,
        IRequestHandler<GetAllPaginatedValueCommandRequest, GetAllPaginatedValueCommandResponse>
    {
        public readonly IValueService<ValueEntity> _valueService = entitiesService;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreateValueCommandResponse> Handle(CreateValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var valueEntity = await MapValue(request.Value.ValueRequest, Guid.NewGuid(), true);
                await _valueService.InsertAsync(valueEntity);

                return new CreateValueCommandResponse(
                    new ValueCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new ValueCreate
                        {
                            Id = valueEntity.id,
                            Code = valueEntity.value_code,
                            Name = valueEntity.value_name,
                            TypeId = valueEntity.type_id,
                            StatusId = valueEntity.status_id
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

        public async Task<UpdateValueCommandResponse> Handle(UpdateValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var valueById = await _valueService.GetByIdAsync(request.Value.ValueRequest.Id);
                if (valueById == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                         new DetailsArgumentErrors()
                         {
                             Code = (int)ResponseCode.NotFoundSuccessfully,
                             Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                             Data = request.Value.ValueRequest
                         });
                }

                var valueEntity = await MapValue(request.Value.ValueRequest, request.Value.ValueRequest.Id);
                await _valueService.UpdateAsync(valueEntity);

                return new UpdateValueCommandResponse(
                        new ValueUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new ValueUpdate
                            {
                                Id = valueEntity.id,
                                Code = valueById.value_code,
                                Name = valueEntity.value_name,
                                TypeId = valueEntity.type_id,
                                StatusId = valueEntity.status_id
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

        public async Task<DeleteValueCommandResponse> Handle(DeleteValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var valueById = await _valueService.GetByIdAsync(request.Value.Id);
                if (valueById == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Value
                        });
                }

                await _valueService.DeleteAsync(valueById);

                return new DeleteValueCommandResponse(
                    new ValueDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new ValueDelete
                        {
                            Id = valueById.id
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

        public async Task<GetByIdValueCommandResponse> Handle(GetByIdValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var valueById = await _valueService.GetByIdAsync(request.Value.Id);
                if (valueById == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Value
                        });
                }

                return new GetByIdValueCommandResponse(
                    new ValueGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new ValueGetById
                        {
                            Id = valueById.id,
                            Name = valueById.value_name,
                            Code = valueById.value_code,
                            TypeId = valueById.type_id,
                            StatusId = valueById.status_id
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

        public async Task<GetByCodeValueCommandResponse> Handle(GetByCodeValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesByCode = await _valueService.GetByCodeAsync(request.Value.Code);
                if (entitiesByCode == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Value
                        });
                }

                return new GetByCodeValueCommandResponse(
                    new ValueGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new ValueGetByCode
                        {
                            Id = entitiesByCode.id,
                            Name = entitiesByCode.value_name,
                            Code = entitiesByCode.value_code,
                            TypeId = entitiesByCode.type_id,
                            StatusId = entitiesByCode.status_id
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

        public async Task<GetByTypeValueCommandResponse> Handle(GetByTypeValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesByType = await _valueService.GetByTypeAsync(request.Value.TypeId);
                if (entitiesByType == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Value
                        });
                }

                return new GetByTypeValueCommandResponse(
                    new ValueGetByTypeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = entitiesByType.Select(c => new ValueGetByType
                        {
                            Id = c.id,
                            Name = c.value_name,
                            Code = c.value_code,
                            TypeId = c.type_id,
                            StatusId = c.status_id
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

        public async Task<GetAllPaginatedValueCommandResponse> Handle(GetAllPaginatedValueCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Value.Adapt<PaginatedModel>();
                var rows = await _valueService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
                        });
                }
                var result = await _valueService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedValueCommandResponse(
                    new ValueGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new ValueGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new ValueGetAllPaginated
                            {
                                Id = c.id,
                                Name = c.value_name,
                                Code = c.value_code,
                                TypeId = c.type_id,
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

        private async Task<ValueEntity> MapValue(ValueCreateRequest request, Guid id, bool? create = null)
        {
            var entitiesEntity = new ValueEntity()
            {
                id = id,
                value_name = request.Name,
                value_code = create == true
                ? await _codeConfiguratorService.GenerateCodeAsync(Modules.Value)
                : null,
                type_id = request.TypeId,
                status_id = request.StatusId
            };
            return entitiesEntity;
        }
    }
}
