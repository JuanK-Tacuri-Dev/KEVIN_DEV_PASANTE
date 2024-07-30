using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog.CatalogCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog.Validators
{
    public class UpdateCatalogCommandRequestValidator : AbstractValidator<UpdateCatalogCommandRequest>
    {
        public UpdateCatalogCommandRequestValidator()
        {
            /*RuleFor(request => request.Catalog.CatalogRequest.CatalogCode)
            .NotEmpty().WithMessage(AppMessages.Catalog_CatalogCode_Required);

            RuleFor(request => request.Catalog.CatalogRequest.Type)
            .NotEmpty().WithMessage(AppMessages.Catalog_Type_Required);

            RuleFor(request => request.Catalog.CatalogRequest.ConnectionId)
            .NotEmpty().WithMessage(AppMessages.Catalog_ConnectionId_Required);

            RuleFor(request => request.Catalog.CatalogRequest.Entities)
            .NotEmpty().WithMessage(AppMessages.Catalog_Objects_Required);*/
        }
    }
}
