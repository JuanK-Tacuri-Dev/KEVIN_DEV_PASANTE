using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Adapter.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateAdapterCommandRequestValidator : AbstractValidator<UpdateAdapterCommandRequest>
    {
        public UpdateAdapterCommandRequestValidator()
        {
            RuleFor(request => request.Adapter.AdapterRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
            
            RuleFor(request => request.Adapter.AdapterRequest.GetType())
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            
        }
    }
}
