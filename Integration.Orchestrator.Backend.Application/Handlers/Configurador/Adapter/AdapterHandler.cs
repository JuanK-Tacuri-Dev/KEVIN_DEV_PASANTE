using Integration.Orchestrator.Backend.Application.Models.Configurador.Adapter;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterHandler(IAdapterService<AdapterEntity> adapterService, IConnectionService<ConnectionEntity> connectionService, IStatusService<StatusEntity> statusService)
    #region MediateR
        :
        IRequestHandler<CreateAdapterCommandRequest, CreateAdapterCommandResponse>,
        IRequestHandler<UpdateAdapterCommandRequest, UpdateAdapterCommandResponse>,
        IRequestHandler<DeleteAdapterCommandRequest, DeleteAdapterCommandResponse>,
        IRequestHandler<GetByIdAdapterCommandRequest, GetByIdAdapterCommandResponse>,
        IRequestHandler<GetByCodeAdapterCommandRequest, GetByCodeAdapterCommandResponse>,
        IRequestHandler<GetByTypeAdapterCommandRequest, GetByTypeAdapterCommandResponse>,
        IRequestHandler<GetAllPaginatedAdapterCommandRequest, GetAllPaginatedAdapterCommandResponse>
    {
        #endregion
        private readonly IAdapterService<AdapterEntity> _adapterService = adapterService;
        private readonly IConnectionService<ConnectionEntity> _connectionService = connectionService;
        public readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task<CreateAdapterCommandResponse> Handle(CreateAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterMap = MapAdapter(request.Adapter.AdapterRequest, Guid.NewGuid());
                await _adapterService.InsertAsync(adapterMap);

                return new CreateAdapterCommandResponse(
                    new AdapterCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new AdapterCreate
                        {
                            Id = adapterMap.id,
                            Code = adapterMap.adapter_code,
                            Name = adapterMap.adapter_name,
                            TypeAdapterId = adapterMap.type_id,
                            Version = adapterMap.adapter_version,
                            StatusId = adapterMap.status_id
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

        public async Task<UpdateAdapterCommandResponse> Handle(UpdateAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterFound = await _adapterService.GetByIdAsync(request.Id);
                if (adapterFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Adapter.AdapterRequest
                        });

                var adapterMap = MapAdapter(request.Adapter.AdapterRequest, request.Id);

                var StatusIsActive = await _statusService.GetStatusIsActive(adapterMap.status_id);
                var ExistRelationConection = await _connectionService.GetByAdapterIdAsync(adapterMap.id);

                if (!StatusIsActive && ExistRelationConection != null)
                {
                    var StatusConectionActive = await _statusService.GetStatusIsActive(ExistRelationConection.status_id);
                    if (StatusConectionActive)
                    {
                        throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.CannotDeleteDueToRelationship,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.CannotDeleteDueToRelationship),
                            Data = request.Adapter
                        });
                    }
                }

                await _adapterService.UpdateAsync(adapterMap);

                return new UpdateAdapterCommandResponse(
                        new AdapterUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new AdapterUpdate
                            {
                                Id = adapterMap.id,
                                Code = adapterFound.adapter_code,
                                Name = adapterMap.adapter_name,
                                TypeAdapterId = adapterMap.type_id,
                                Version = adapterMap.adapter_version,
                                StatusId = adapterMap.status_id
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

        public async Task<DeleteAdapterCommandResponse> Handle(DeleteAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterFound = await _adapterService.GetByIdAsync(request.Adapter.Id);
                if (adapterFound == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Adapter
                        });
                }

                var ExistConection = _connectionService.GetByAdapterIdAsync(adapterFound.id);
                if (ExistConection != null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.CannotDeleteDueToRelationship,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.CannotDeleteDueToRelationship),
                            Data = request.Adapter
                        });

                }


                await _adapterService.DeleteAsync(adapterFound);

                return new DeleteAdapterCommandResponse(
                    new AdapterDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new AdapterDelete
                        {
                            Id = adapterFound.id
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

        public async Task<GetByIdAdapterCommandResponse> Handle(GetByIdAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterFound = await _adapterService.GetByIdAsync(request.Adapter.Id);
                if (adapterFound == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Adapter
                        });
                }

                return new GetByIdAdapterCommandResponse(
                    new AdapterGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new AdapterGetById
                        {
                            Id = adapterFound.id,
                            Code = adapterFound.adapter_code,
                            Name = adapterFound.adapter_name,
                            TypeAdapterId = adapterFound.type_id,
                            Version = adapterFound.adapter_version,
                            StatusId = adapterFound.status_id
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

        public async Task<GetByCodeAdapterCommandResponse> Handle(GetByCodeAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterFound = await _adapterService.GetByCodeAsync(request.Adapter.Code);
                if (adapterFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Adapter
                        });

                return new GetByCodeAdapterCommandResponse(
                    new AdapterGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new AdapterGetByCode
                        {
                            Id = adapterFound.id,
                            Name = adapterFound.adapter_name,
                            Code = adapterFound.adapter_code,
                            TypeAdapterId = adapterFound.type_id,
                            Version = adapterFound.adapter_version,
                            StatusId = adapterFound.status_id
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

        public async Task<GetByTypeAdapterCommandResponse> Handle(GetByTypeAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterFound = await _adapterService.GetByTypeAsync(request.Adapter.TypeAdapterId);
                if (adapterFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Adapter
                        });

                return new GetByTypeAdapterCommandResponse(
                    new AdapterGetByTypeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = adapterFound.Select(adapter => new AdapterGetByType
                        {
                            Id = adapter.id,
                            Name = adapter.adapter_name,
                            Code = adapter.adapter_code,
                            TypeAdapterId = adapter.type_id,
                            Version = adapter.adapter_version,
                            StatusId = adapter.status_id
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

        public async Task<GetAllPaginatedAdapterCommandResponse> Handle(GetAllPaginatedAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Adapter.Adapt<PaginatedModel>();
                var rows = await _adapterService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedAdapterCommandResponse(
                    new AdapterGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new AdapterGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<AdapterGetAllPaginated>()
                        }
                    });
                }
                var adaptersFound = await _adapterService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedAdapterCommandResponse(
                    new AdapterGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new AdapterGetAllRows
                        {
                            Total_rows = rows,
                            Rows = adaptersFound.Select(adapter => new AdapterGetAllPaginated
                            {
                                Id = adapter.id,
                                Name = adapter.adapter_name,
                                Code = adapter.adapter_code,
                                TypeAdapterId = adapter.type_id,
                                Version = adapter.adapter_version,
                                StatusId = adapter.status_id
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

        private AdapterEntity MapAdapter(AdapterCreateRequest request, Guid id)
        {
            return new AdapterEntity()
            {
                id = id,
                adapter_name = request.Name?.Trim() ?? string.Empty,
                type_id = request.TypeAdapterId,
                status_id = request.StatusId,
                adapter_version = request.Version?.Trim() ?? string.Empty
            };
        }
    }
}
