using Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Catalog.CatalogCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogHandler(ICatalogService<CatalogEntity> catalogService)
        :
        IRequestHandler<CreateCatalogCommandRequest, CreateCatalogCommandResponse>,
        IRequestHandler<UpdateCatalogCommandRequest, UpdateCatalogCommandResponse>,
        IRequestHandler<DeleteCatalogCommandRequest, DeleteCatalogCommandResponse>,
        IRequestHandler<GetByIdCatalogCommandRequest, GetByIdCatalogCommandResponse>,
        IRequestHandler<GetByFatherCatalogCommandRequest, GetByFatherCatalogCommandResponse>,
        IRequestHandler<GetByCodeCatalogCommandRequest, GetByCodeCatalogCommandResponse>,
        IRequestHandler<GetAllPaginatedCatalogCommandRequest, GetAllPaginatedCatalogCommandResponse>
    {
        public readonly ICatalogService<CatalogEntity> _catalogService = catalogService;

        public async Task<CreateCatalogCommandResponse> Handle(CreateCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogMap = MapCatalog(request.Catalog.CatalogRequest, Guid.NewGuid(), true);
                await _catalogService.InsertAsync(catalogMap);

                return new CreateCatalogCommandResponse(
                    new CatalogCreateResponse
                    {
                        Code = (int)ResponseCode.CreatedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.CreatedSuccessfully)],
                        Data = new CatalogCreate
                        {
                            Id = catalogMap.id,
                            Code = catalogMap.catalog_code,
                            Name = catalogMap.catalog_name,
                            Value = catalogMap.catalog_value,
                            FatherCode = catalogMap.father_code,
                            Detail = catalogMap.catalog_detail,
                            IsFather = catalogMap.is_father,
                            StatusId = catalogMap.status_id
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
                var catalogFound = await _catalogService.GetByIdAsync(request.Id);
                if (catalogFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Catalog.CatalogRequest
                        });

                var catalogMap = MapCatalog(request.Catalog.CatalogRequest, request.Id);
                await _catalogService.UpdateAsync(catalogMap);

                return new UpdateCatalogCommandResponse(
                        new CatalogUpdateResponse
                        {
                            Code = (int)ResponseCode.UpdatedSuccessfully,
                            Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.UpdatedSuccessfully)],
                            Data = new CatalogUpdate
                            {
                                Id = catalogMap.id,
                                Code = catalogFound.catalog_code,
                                Name = catalogMap.catalog_name,
                                Value = catalogMap.catalog_value,
                                FatherCode = catalogMap.father_code,
                                Detail = catalogMap.catalog_detail,
                                IsFather = catalogMap.is_father,
                                StatusId = catalogMap.status_id
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
                var cataloFound = await _catalogService.GetByIdAsync(request.Catalog.Id);
                if (cataloFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Catalog
                        });

                await _catalogService.DeleteAsync(cataloFound);

                return new DeleteCatalogCommandResponse(
                    new CatalogDeleteResponse
                    {
                        Code = (int)ResponseCode.DeletedSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.DeletedSuccessfully)],
                        Data = new CatalogDelete
                        {
                            Id = cataloFound.id,
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
                var catalogFound = await _catalogService.GetByIdAsync(request.Catalog.Id);
                if (catalogFound == null)
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
                            Id = catalogFound.id,
                            Code = catalogFound.catalog_code,
                            Name = catalogFound.catalog_name,
                            Value = catalogFound.catalog_value,
                            FatherCode = catalogFound.father_code,
                            Detail = catalogFound.catalog_detail,
                            IsFather = catalogFound.is_father,
                            StatusId = catalogFound.status_id
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
                var catalogFound = await _catalogService.GetByFatherAsync(request.Catalog.FatherCode);
                if (catalogFound == null)
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
                        Data = catalogFound.Select(catalog => new CatalogGetByType
                        {
                            Id = catalog.id,
                            Code = catalog.catalog_code,
                            Name = catalog.catalog_name,
                            Value = catalog.catalog_value,
                            FatherCode = catalog.father_code,
                            Detail = catalog.catalog_detail,
                            IsFather = catalog.is_father,
                            StatusId = catalog.status_id
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

        public async Task<GetByCodeCatalogCommandResponse> Handle(GetByCodeCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogFound = await _catalogService.GetByCodeAsync(request.Catalog.Code);
                if (catalogFound == null)
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                            Data = request.Catalog
                        });

                return new GetByCodeCatalogCommandResponse(
                    new CatalogGetByCodeResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Messages = [ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully)],
                        Data = new CatalogGetByCode
                        {
                            Id = catalogFound.id,
                            Code = catalogFound.catalog_code,
                            Name = catalogFound.catalog_name,
                            Value = catalogFound.catalog_value,
                            FatherCode = catalogFound.father_code,
                            Detail = catalogFound.catalog_detail,
                            IsFather = catalogFound.is_father,
                            StatusId = catalogFound.status_id
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

        public async Task<GetAllPaginatedCatalogCommandResponse> Handle(GetAllPaginatedCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Catalog.Adapt<PaginatedModel>();
                var rows = await _catalogService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedCatalogCommandResponse(
                    new CatalogGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new CatalogGetAllRows
                        {
                            Total_rows = rows,
                            Rows = Enumerable.Empty<CatalogGetAllPaginated>()
                        }
                    });
                }
                var catalogsFound = await _catalogService.GetAllPaginatedAsync(model);

                return new GetAllPaginatedCatalogCommandResponse(
                    new CatalogGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new CatalogGetAllRows
                        {
                            Total_rows = rows,
                            Rows = catalogsFound.Select(catalog => new CatalogGetAllPaginated
                            {
                                Id = catalog.id,
                                Code = catalog.catalog_code,
                                Name = catalog.catalog_name,
                                Value = catalog.catalog_value,
                                FatherCode = catalog.father_code,
                                Detail = catalog.catalog_detail,
                                IsFather = catalog.is_father,
                                StatusId = catalog.status_id
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

        private CatalogEntity MapCatalog(CatalogCreateRequest request, Guid id, bool? create = null)
        {
            return new CatalogEntity()
            {
                id = id,
                catalog_code = request.Code,
                catalog_name = request.Name?.Trim() ?? string.Empty,
                catalog_value = request.Value?.Trim() ?? string.Empty,
                catalog_detail = request.Detail?.Trim() ?? string.Empty,
                father_code = request.FatherCode,
                status_id = request.StatusId
            };
        }
    }
}
