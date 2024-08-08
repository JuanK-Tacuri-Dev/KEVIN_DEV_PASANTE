using Integration.Orchestrator.Backend.Application.Models.Administration.Adapter;
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
        IModuleSequenceService moduleSequenceService)
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
        private readonly IModuleSequenceService _moduleSequenceService = moduleSequenceService;

        public async Task<CreateAdapterCommandResponse> Handle(CreateAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterEntity = await MapAdapter(request.Adapter.AdapterRequest, Guid.NewGuid());
                await _adapterService.InsertAsync(adapterEntity);

                return new CreateAdapterCommandResponse(
                    new AdapterCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new AdapterCreate
                        {
                            Id = adapterEntity.id,
                            Code = adapterEntity.adapter_code,
                            Name = adapterEntity.name,
                            TypeAdapterId = adapterEntity.adapter_type_id,
                            Version = adapterEntity.version,
                            StatusId = adapterEntity.status_id
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
                var adapterById = await _adapterService.GetByIdAsync(request.Id);
                if (adapterById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Adapter.AdapterRequest
                        });

                var adapterEntity = await MapAdapter(request.Adapter.AdapterRequest, request.Id);
                await _adapterService.UpdateAsync(adapterEntity);

                return new UpdateAdapterCommandResponse(
                        new AdapterUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new AdapterUpdate
                            {
                                Id = adapterEntity.id,
                                Code = adapterEntity.adapter_code,
                                Name = adapterEntity.name,
                                TypeAdapterId = adapterEntity.adapter_type_id,
                                Version = adapterEntity.version,
                                StatusId = adapterEntity.status_id
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
                var adapterById = await _adapterService.GetByIdAsync(request.Adapter.Id);
                if (adapterById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Adapter
                        });

                await _adapterService.DeleteAsync(adapterById);

                return new DeleteAdapterCommandResponse(
                    new AdapterDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new AdapterDelete 
                        {
                            Id = adapterById.id
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
                var adapterById = await _adapterService.GetByIdAsync(request.Adapter.Id);
                if (adapterById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Adapter
                        });

                return new GetByIdAdapterCommandResponse(
                    new AdapterGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new AdapterGetById
                        {
                            Id = adapterById.id,
                            Code = adapterById.adapter_code,
                            Name = adapterById.name,
                            TypeAdapterId = adapterById.adapter_type_id,
                            Version = adapterById.version,
                            StatusId = adapterById.status_id
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
                var adapterByCode = await _adapterService.GetByCodeAsync(request.Adapter.Code);
                if (adapterByCode == null)
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
                            Id = adapterByCode.id,
                            Name = adapterByCode.name,
                            Code = adapterByCode.adapter_code,
                            TypeAdapterId = adapterByCode.adapter_type_id,
                            Version = adapterByCode.version,
                            StatusId = adapterByCode.status_id
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
                var adapterByType = await _adapterService.GetByTypeAsync(request.Adapter.TypeAdapterId);
                if (adapterByType == null)
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
                        Data = adapterByType.Select(c => new AdapterGetByType
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.adapter_code,
                            TypeAdapterId = c.adapter_type_id,
                            Version = c.version,
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
                var result = await _adapterService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedAdapterCommandResponse(
                    new AdapterGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new AdapterGetAllRows 
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new AdapterGetAllPaginated
                            {
                                Id = c.id,
                                Name = c.name,
                                Code = c.adapter_code,
                                TypeAdapterId = c.adapter_type_id,
                                Version = c.version,
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

        private async Task<AdapterEntity> MapAdapter(AdapterCreateRequest request, Guid id)
        {
            var adapterEntity = new AdapterEntity()
            {
                id = id,
                adapter_code = await _moduleSequenceService.GenerateCodeAsync(Modules.Adapter.ToString()),
                name = request.Name,
                adapter_type_id = request.TypeAdapterId,
                status_id = request.StatusId,
                version = request.Version
                
            };
            return adapterEntity;
        }
    }
}
