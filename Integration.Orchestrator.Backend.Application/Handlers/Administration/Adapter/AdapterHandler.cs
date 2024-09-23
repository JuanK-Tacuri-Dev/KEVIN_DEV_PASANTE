﻿using Integration.Orchestrator.Backend.Application.Models.Administration.Adapter;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Adapter
{
    public class AdapterHandler(
        IAdapterService<AdapterEntity> adapterService,
        ICodeConfiguratorService codeConfiguratorService)
        :
        IRequestHandler<CreateAdapterCommandRequest, CreateAdapterCommandResponse>,
        IRequestHandler<UpdateAdapterCommandRequest, UpdateAdapterCommandResponse>,
        IRequestHandler<DeleteAdapterCommandRequest, DeleteAdapterCommandResponse>,
        IRequestHandler<GetByIdAdapterCommandRequest, GetByIdAdapterCommandResponse>,
        IRequestHandler<GetByCodeAdapterCommandRequest, GetByCodeAdapterCommandResponse>,
        IRequestHandler<GetByTypeAdapterCommandRequest, GetByTypeAdapterCommandResponse>,
        IRequestHandler<GetAllPaginatedAdapterCommandRequest, GetAllPaginatedAdapterCommandResponse>
    {
        private readonly IAdapterService<AdapterEntity> _adapterService = adapterService;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;

        public async Task<CreateAdapterCommandResponse> Handle(CreateAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterMap = await MapAdapter(request.Adapter.AdapterRequest, Guid.NewGuid(), true);
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

                var adapterMap = await MapAdapter(request.Adapter.AdapterRequest, request.Id);
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
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
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

        private async Task<AdapterEntity> MapAdapter(AdapterCreateRequest request, Guid id, bool? create = null)
        {
            return new AdapterEntity()
            {
                id = id,
                adapter_code = create == true
                ? await _codeConfiguratorService.GenerateCodeAsync(Modules.Adapter)
                : null,
                adapter_name = request.Name,
                type_id = request.TypeAdapterId,
                status_id = request.StatusId,
                adapter_version = request.Version
            };
        }
    }
}
