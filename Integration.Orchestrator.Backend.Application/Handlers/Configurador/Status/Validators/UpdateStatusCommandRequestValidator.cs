using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateStatusCommandRequestValidator : AbstractValidator<UpdateStatusCommandRequest>
    {
        public UpdateStatusCommandRequestValidator()
        {
            RuleFor(request => request.Status.StatusRequest.Key)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Status.StatusRequest.Text)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Status.StatusRequest.Color)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
