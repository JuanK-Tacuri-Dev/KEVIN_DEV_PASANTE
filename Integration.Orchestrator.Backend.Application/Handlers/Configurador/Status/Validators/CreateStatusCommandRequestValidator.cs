using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status.Validators
{
    public class CreateStatusCommandRequestValidator : AbstractValidator<CreateStatusCommandRequest>
    {
        public CreateStatusCommandRequestValidator()
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
