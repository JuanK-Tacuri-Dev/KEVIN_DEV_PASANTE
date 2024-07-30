using Integration.Orchestrator.Backend.Application.Models.Administration.Catalog;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Resources;
using Mapster;
using MediatR;
using System.Net;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog.CatalogCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Catalog
{
    public class CatalogHandler(ICatalogService<CatalogEntity> catalogService)
        :
        IRequestHandler<CreateCatalogCommandRequest, CreateCatalogCommandResponse>,
        IRequestHandler<UpdateCatalogCommandRequest, UpdateCatalogCommandResponse>,
        IRequestHandler<DeleteCatalogCommandRequest, DeleteCatalogCommandResponse>,
        IRequestHandler<GetByIdCatalogCommandRequest, GetByIdCatalogCommandResponse>,
        IRequestHandler<GetByTypeCatalogCommandRequest, GetByTypeCatalogCommandResponse>,
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
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeCreated],
                        Data = new CatalogCreate
                        {
                            Id = catalogEntity.id,
                            Name = catalogEntity.name,
                            Value = catalogEntity.value,
                            Type = catalogEntity.catalog_Type,
                            FatherId = catalogEntity.id,
                            StatusId = catalogEntity.status_id
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

        public async Task<UpdateCatalogCommandResponse> Handle(UpdateCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogById = await _catalogService.GetByIdAsync(request.Id);
                if (catalogById == null)
                {
                    throw new ArgumentException(AppMessages.Application_CatalogNotFound);
                }

                var catalogEntity = MapCatalog(request.Catalog.CatalogRequest, request.Id);
                await _catalogService.UpdateAsync(catalogEntity);

                return new UpdateCatalogCommandResponse(
                        new CatalogUpdateResponse
                        {
                            Code = HttpStatusCode.OK.GetHashCode(),
                            Messages = [AppMessages.Application_RespondeUpdated],
                            Data = new CatalogUpdate
                            {
                                Id = catalogEntity.id,
                                Name = catalogEntity.name,
                                Value = catalogEntity.value,
                                Type = catalogEntity.catalog_Type,
                                FatherId = catalogEntity.id,
                                StatusId = catalogEntity.status_id
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

        public async Task<DeleteCatalogCommandResponse> Handle(DeleteCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogById = await _catalogService.GetByIdAsync(request.Catalog.Id);
                if (catalogById == null)
                {
                    throw new ArgumentException(AppMessages.Application_CatalogNotFound);
                }

                await _catalogService.DeleteAsync(catalogById);

                return new DeleteCatalogCommandResponse(
                    new CatalogDeleteResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeDeleted],
                        Data = new CatalogDelete
                        {
                            Id = catalogById.id,
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

        public async Task<GetByIdCatalogCommandResponse> Handle(GetByIdCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogById = await _catalogService.GetByIdAsync(request.Catalog.Id);
                if (catalogById == null)
                {
                    throw new ArgumentException(AppMessages.Application_CatalogNotFound);
                }

                return new GetByIdCatalogCommandResponse(
                    new CatalogGetByIdResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = new CatalogGetById
                        {
                            Id = catalogById.id,
                            Name = catalogById.name,
                            Value = catalogById.value,
                            Type = catalogById.catalog_Type,
                            FatherId = catalogById.id,
                            StatusId = catalogById.status_id
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

        public async Task<GetByTypeCatalogCommandResponse> Handle(GetByTypeCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var catalogByType = await _catalogService.GetByTypeAsync(request.Catalog.Type);
                if (catalogByType == null)
                {
                    throw new ArgumentException(AppMessages.Application_CatalogNotFound);
                }

                return new GetByTypeCatalogCommandResponse(
                    new CatalogGetByTypeResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Messages = [AppMessages.Application_RespondeGet],
                        Data = catalogByType.Select(p => new CatalogGetByType
                        {
                            Id = p.id,
                            Name = p.name,
                            Value = p.value,
                            Type = p.catalog_Type,
                            FatherId = p.id,
                            StatusId = p.status_id
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

        public async Task<GetAllPaginatedCatalogCommandResponse> Handle(GetAllPaginatedCatalogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Catalog.Adapt<PaginatedModel>();
                var rows = await _catalogService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    throw new ArgumentException(AppMessages.Application_CatalogNotFound);
                }
                var result = await _catalogService.GetAllPaginatedAsync(model);


                return new GetAllPaginatedCatalogCommandResponse(
                    new CatalogGetAllPaginatedResponse
                    {
                        Code = HttpStatusCode.OK.GetHashCode(),
                        Description = AppMessages.Application_RespondeGetAll,
                        Data = new CatalogGetAllRows
                        {
                            Total_rows = rows,
                            Rows = result.Select(p => new CatalogGetAllPaginated
                            {
                                Id = p.id,
                                Name = p.name,
                                Value = p.value,
                                Type = p.catalog_Type,
                                FatherId = p.id,
                                StatusId = p.status_id
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

        private CatalogEntity MapCatalog(CatalogCreateRequest request, Guid id)
        {
            var catalogEntity = new CatalogEntity()
            {
                id = id,
                name = request.Name,
                value = request.Value,
                catalog_Type = request.Type,
                father_id = request.FatherId,
                status_id = request.StatusId
            };
            return catalogEntity;
        }
    }
}
