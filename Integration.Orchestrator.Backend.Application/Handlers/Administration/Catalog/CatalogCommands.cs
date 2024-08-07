using Integration.Orchestrator.Backend.Application.Models.Administration.Catalog;
using MediatR;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog
{
    public class CatalogCommands
    {
        public readonly record struct CreateCatalogCommandRequest(CatalogBasicInfoRequest<CatalogCreateRequest> Catalog) : IRequest<CreateCatalogCommandResponse>;
        public readonly record struct CreateCatalogCommandResponse(CatalogCreateResponse Message);

        public readonly record struct UpdateCatalogCommandRequest(CatalogBasicInfoRequest<CatalogUpdateRequest> Catalog, Guid Id) : IRequest<UpdateCatalogCommandResponse>;
        public readonly record struct UpdateCatalogCommandResponse(CatalogUpdateResponse Message);

        public readonly record struct DeleteCatalogCommandRequest(CatalogDeleteRequest Catalog) : IRequest<DeleteCatalogCommandResponse>;
        public readonly record struct DeleteCatalogCommandResponse(CatalogDeleteResponse Message);

        public readonly record struct GetByIdCatalogCommandRequest(CatalogGetByIdRequest Catalog) : IRequest<GetByIdCatalogCommandResponse>;
        public readonly record struct GetByIdCatalogCommandResponse(CatalogGetByIdResponse Message);

        public readonly record struct GetByFatherCatalogCommandRequest(CatalogGetByFatherRequest Catalog) : IRequest<GetByFatherCatalogCommandResponse>;
        public readonly record struct GetByFatherCatalogCommandResponse(CatalogGetByFatherResponse Message);

        public readonly record struct GetAllPaginatedCatalogCommandRequest(CatalogGetAllPaginatedRequest Catalog) : IRequest<GetAllPaginatedCatalogCommandResponse>;
        public readonly record struct GetAllPaginatedCatalogCommandResponse(CatalogGetAllPaginatedResponse Message);
    }
}
