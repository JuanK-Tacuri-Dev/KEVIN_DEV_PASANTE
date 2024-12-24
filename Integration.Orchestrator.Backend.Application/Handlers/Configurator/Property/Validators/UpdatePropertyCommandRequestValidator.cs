using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Property.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdatePropertyCommandRequestValidator : AbstractValidator<UpdatePropertyCommandRequest>
    {
        public UpdatePropertyCommandRequestValidator()
        {
            RuleFor(request => request.Property.PropertyRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Property.PropertyRequest.TypeId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            
        }
    }
}
