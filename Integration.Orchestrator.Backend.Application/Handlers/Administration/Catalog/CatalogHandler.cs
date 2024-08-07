using Integration.Orchestrator.Backend.Application.Models.Administration.Catalog;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog.CatalogCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog
{
    public class CatalogHandler(ICatalogService<CatalogEntity> catalogService)
        :
        IRequestHandler<CreateCatalogCommandRequest, CreateCatalogCommandResponse>,
        IRequestHandler<UpdateCatalogCommandRequest, UpdateCatalogCommandResponse>,
        IRequestHandler<DeleteCatalogCommandRequest, DeleteCatalogCommandResponse>,
        IRequestHandler<GetByIdCatalogCommandRequest, GetByIdCatalogCommandResponse>,
        IRequestHandler<GetByFatherCatalogCommandRequest, GetByFatherCatalogCommandResponse>,
        IRequestHandler<GetAllPaginatedCatalogCommandRequest, GetAllPaginatedCatalogCommandResponse>
    {
        public readonly ICatalogService<CatalogEntity> _catalogService = catalogService;

        public async Task<CreateCatalogCommandResponse> Handle(CreateCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogEntity = MapCatalog(request.Catalog.CatalogRequest, Guid.NewGuid());
                await _catalogService.InsertAsync(catalogEntity);

                return new CreateCatalogCommandResponse(
                    new CatalogCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new CatalogCreate
                        {
                            Id = catalogEntity.id,
                            Name = catalogEntity.name,
                            Value = catalogEntity.value,
                            FatherId = catalogEntity.father_id,
                            Detail = catalogEntity.detail,
                            StatusId = catalogEntity.status_id
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

        public async Task<UpdateCatalogCommandResponse> Handle(UpdateCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogById = await _catalogService.GetByIdAsync(request.Id);
                if (catalogById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Catalog.CatalogRequest
                        });

                var catalogEntity = MapCatalog(request.Catalog.CatalogRequest, request.Id);
                await _catalogService.UpdateAsync(catalogEntity);

                return new UpdateCatalogCommandResponse(
                        new CatalogUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new CatalogUpdate
                            {
                                Id = catalogEntity.id,
                                Name = catalogEntity.name,
                                Value = catalogEntity.value,
                                FatherId = catalogEntity.father_id,
                                Detail = catalogEntity.detail,
                                StatusId = catalogEntity.status_id
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

        public async Task<DeleteCatalogCommandResponse> Handle(DeleteCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogById = await _catalogService.GetByIdAsync(request.Catalog.Id);
                if (catalogById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Catalog
                        });

                await _catalogService.DeleteAsync(catalogById);

                return new DeleteCatalogCommandResponse(
                    new CatalogDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new CatalogDelete
                        {
                            Id = catalogById.id,
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

        public async Task<GetByIdCatalogCommandResponse> Handle(GetByIdCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogById = await _catalogService.GetByIdAsync(request.Catalog.Id);
                if (catalogById == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Catalog
                        });

                return new GetByIdCatalogCommandResponse(
                    new CatalogGetByIdResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new CatalogGetById
                        {
                            Id = catalogById.id,
                            Name = catalogById.name,
                            Value = catalogById.value,
                            FatherId = catalogById.father_id,
                            Detail = catalogById.detail,
                            StatusId = catalogById.status_id
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

        public async Task<GetByFatherCatalogCommandResponse> Handle(GetByFatherCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogByType = await _catalogService.GetByFatherAsync(request.Catalog.FatherId);
                if (catalogByType == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Catalog
                        });

                return new GetByFatherCatalogCommandResponse(
                    new CatalogGetByFatherResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = catalogByType.Select(p => new CatalogGetByType
                        {
                            Id = p.id,
                            Name = p.name,
                            Value = p.value,
                            FatherId = p.father_id,
                            Detail = p.detail,
                            StatusId = p.status_id
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

        public async Task<GetAllPaginatedCatalogCommandResponse> Handle(GetAllPaginatedCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Catalog.Adapt<PaginatedModel>();
                var rows = await _catalogService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully)
                        });
                }
                var result = await _catalogService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedCatalogCommandResponse(
                    new CatalogGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new CatalogGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(p => new CatalogGetAllPaginated
                            {
                                Id = p.id,
                                Name = p.name,
                                Value = p.value,
                                FatherId = p.father_id,
                                Detail = p.detail,
                                StatusId = p.status_id
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

        private CatalogEntity MapCatalog(CatalogCreateRequest request, Guid id)
        {
            var catalogEntity = new CatalogEntity()
            {
                id = id,
                name = request.Name,
                value = request.Value,
                detail = request.Detail,
                father_id = request.FatherId,
                status_id = request.StatusId
            };
            return catalogEntity;
        }
    }
}
