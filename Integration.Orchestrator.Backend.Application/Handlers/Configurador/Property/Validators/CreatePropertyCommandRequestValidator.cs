using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Property.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreatePropertyCommandRequestValidator : AbstractValidator<CreatePropertyCommandRequest>
    {
        public CreatePropertyCommandRequestValidator()
        {
            RuleFor(request => request.Property.PropertyRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
            
            RuleFor(request => request.Property.PropertyRequest.TypeId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            
        }
    }
}
