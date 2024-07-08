using Integration.Orchestrator.Backend.Application.Models.Administration.Adapter;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Adapter
{
    public class AdapterHandler(IAdapterService<AdapterEntity> adapterService)
        :
        IRequestHandler<CreateAdapterCommandRequest, CreateAdapterCommandResponse>,
        IRequestHandler<UpdateAdapterCommandRequest, UpdateAdapterCommandResponse>,
        IRequestHandler<DeleteAdapterCommandRequest, DeleteAdapterCommandResponse>,
        IRequestHandler<GetByIdAdapterCommandRequest, GetByIdAdapterCommandResponse>,
        IRequestHandler<GetByCodeAdapterCommandRequest, GetByCodeAdapterCommandResponse>,
        IRequestHandler<GetByTypeAdapterCommandRequest, GetByTypeAdapterCommandResponse>,
        IRequestHandler<GetAllPaginatedAdapterCommandRequest, GetAllPaginatedAdapterCommandResponse>
    {
        public readonly IAdapterService<AdapterEntity> _adapterService = adapterService;

        public async Task<CreateAdapterCommandResponse> Handle(CreateAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterEntity = MapAdapter(request.Adapter.AdapterRequest, Guid.NewGuid());
                await _adapterService.InsertAsync(adapterEntity);

                return new CreateAdapterCommandResponse(
                    new AdapterCreateResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_AdapterResponseCreated,
                        Data = new AdapterCreate()
                        {
                            Id = adapterEntity.id
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

        public async Task<UpdateAdapterCommandResponse> Handle(UpdateAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterById = await _adapterService.GetByIdAsync(request.Id);
                if (adapterById == null)
                {
                    throw new ArgumentException(AppMessages.Application_AdapterNotFound);
                }

                var adapterEntity = MapAdapter(request.Adapter.AdapterRequest, request.Id);
                await _adapterService.UpdateAsync(adapterEntity);

                return new UpdateAdapterCommandResponse(
                        new AdapterUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Description = AppMessages.Application_AdapterResponseUpdated,
                            Data = new AdapterUpdate()
                            {
                                Id = adapterEntity.id
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

        public async Task<DeleteAdapterCommandResponse> Handle(DeleteAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterById = await _adapterService.GetByIdAsync(request.Adapter.Id);
                if (adapterById == null)
                {
                    throw new ArgumentException(AppMessages.Application_AdapterNotFound);
                }

                await _adapterService.DeleteAsync(adapterById);

                return new DeleteAdapterCommandResponse(
                    new AdapterDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_AdapterResponseDeleted
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

        public async Task<GetByIdAdapterCommandResponse> Handle(GetByIdAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterById = await _adapterService.GetByIdAsync(request.Adapter.Id);
                if (adapterById == null)
                {
                    throw new ArgumentException(AppMessages.Application_AdapterNotFound);
                }

                return new GetByIdAdapterCommandResponse(
                    new AdapterGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_AdapterResponse,
                        Data = new AdapterGetById
                        {
                            Id = adapterById.id,
                            Name = adapterById.name,
                            Code = adapterById.adapter_code,
                            Type = adapterById.adapter_type
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

        public async Task<GetByCodeAdapterCommandResponse> Handle(GetByCodeAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterByCode = await _adapterService.GetByCodeAsync(request.Adapter.Code);
                if (adapterByCode == null)
                {
                    throw new ArgumentException(AppMessages.Application_AdapterNotFound);
                }

                return new GetByCodeAdapterCommandResponse(
                    new AdapterGetByCodeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_AdapterResponse,
                        Data = new AdapterGetByCode
                        {
                            Id = adapterByCode.id,
                            Name = adapterByCode.name,
                            Code = adapterByCode.adapter_code,
                            Type = adapterByCode.adapter_type
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

        public async Task<GetByTypeAdapterCommandResponse> Handle(GetByTypeAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var adapterByType = await _adapterService.GetByTypeAsync(request.Adapter.Type);
                if (adapterByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_AdapterNotFound);
                }

                return new GetByTypeAdapterCommandResponse(
                    new AdapterGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_AdapterResponse,
                        Data = adapterByType.Select(c => new AdapterGetByType
                        {
                            Id = c.id,
                            Name = c.name,
                            Code = c.adapter_code,
                            Type = c.adapter_type
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

        public async Task<GetAllPaginatedAdapterCommandResponse> Handle(GetAllPaginatedAdapterCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Adapter.Adapt<PaginatedModel>();
                var rows = await _adapterService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_AdapterNotFound);
                }
                var result = await _adapterService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedAdapterCommandResponse(
                    new AdapterGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Api_AdapterResponse,
                       // TotalRows = rows,
                        Data = new AdapterGetAllRows 
                        {
                            Total_rows = rows,
                            Rows = result.Select(c => new AdapterGetAllPaginated
                            {
                                Id = c.id,
                                Name = c.name,
                                Code = c.adapter_code,
                                Type = c.adapter_type
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

        private AdapterEntity MapAdapter(AdapterCreateRequest request, Guid id)
        {
            var adapterEntity = new AdapterEntity()
            {
                id = id,
                name = request.Name,
                adapter_code = request.Code,
                adapter_type = request.Type
            };
            return adapterEntity;
        }
    }
}
