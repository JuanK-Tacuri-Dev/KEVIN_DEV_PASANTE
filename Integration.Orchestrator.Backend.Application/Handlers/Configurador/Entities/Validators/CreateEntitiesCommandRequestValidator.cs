using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Entities.EntitiesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Entities.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateEntitiesCommandRequestValidator : AbstractValidator<CreateEntitiesCommandRequest>
    {
        public CreateEntitiesCommandRequestValidator()
        {
            RuleFor(request => request.Entities.EntitiesRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Entities_Name_Required);
            
            RuleFor(request => request.Entities.EntitiesRequest.TypeId)
            .NotEmpty().WithMessage(AppMessages.Entities_Type_Required);

            
        }
    }
}
