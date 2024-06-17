using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.Validators
{
    public class CreateSynchronizationStatesCommandRequestValidator : AbstractValidator<CreateSynchronizationStatesCommandRequest>
    {
        public CreateSynchronizationStatesCommandRequestValidator()
        {
            RuleFor(request => request.SynchronizationStates.Name)
            .NotEmpty().WithMessage(AppMessages.SynchronizationStates_Name_Required);

            RuleFor(request => request.SynchronizationStates.Code)
            .NotEmpty().WithMessage(AppMessages.SynchronizationStates_Code_Required);

            RuleFor(request => request.SynchronizationStates.Color)
            .NotEmpty().WithMessage(AppMessages.SynchronizationStates_Color_Required);
        }

    }
}
