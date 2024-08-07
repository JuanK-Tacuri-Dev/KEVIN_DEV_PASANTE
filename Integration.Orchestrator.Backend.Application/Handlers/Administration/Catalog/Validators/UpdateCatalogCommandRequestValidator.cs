using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog.CatalogCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Catalog.Validators
{
    public class UpdateCatalogCommandRequestValidator : AbstractValidator<UpdateCatalogCommandRequest>
    {
        public UpdateCatalogCommandRequestValidator()
        {
            RuleFor(request => request.Catalog.CatalogRequest.Name)
               .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Catalog.CatalogRequest.Value)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Catalog.CatalogRequest.StatusId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
