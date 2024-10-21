using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Adapter.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateAdapterCommandRequestValidator : AbstractValidator<CreateAdapterCommandRequest>
    {
        public CreateAdapterCommandRequestValidator()
        {
            RuleFor(request => request.Adapter.AdapterRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
            
            RuleFor(request => request.Adapter.AdapterRequest.TypeAdapterId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            
        }
    }
}
