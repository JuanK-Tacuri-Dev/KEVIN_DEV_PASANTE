using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status.Validators
{
    public class UpdateStatusCommandRequestValidator : AbstractValidator<UpdateStatusCommandRequest>
    {
        public UpdateStatusCommandRequestValidator()
        {
            RuleFor(request => request.Status.StatusRequest.Key)
            .NotEmpty().WithMessage(AppMessages.Status_Key_Required);

            RuleFor(request => request.Status.StatusRequest.Text)
            .NotEmpty().WithMessage(AppMessages.Status_Text_Required);

            RuleFor(request => request.Status.StatusRequest.Color)
            .NotEmpty().WithMessage(AppMessages.Status_Color_Required);
        }
    }
}
