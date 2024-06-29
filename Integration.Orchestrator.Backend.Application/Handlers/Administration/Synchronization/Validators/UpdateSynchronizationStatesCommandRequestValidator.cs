using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.Validators
{
    public class UpdateSynchronizationStatesCommandRequestValidator : AbstractValidator<UpdateSynchronizationStatesCommandRequest>
    {
        public UpdateSynchronizationStatesCommandRequestValidator()
        {
            RuleFor(request => request.SynchronizationStates.SynchronizationStatesRequest.Name)
            .NotEmpty().WithMessage(AppMessages.SynchronizationStates_Name_Required);

            RuleFor(request => request.SynchronizationStates.SynchronizationStatesRequest.Code)
            .NotEmpty().WithMessage(AppMessages.SynchronizationStates_Code_Required);

            RuleFor(request => request.SynchronizationStates.SynchronizationStatesRequest.Color)
            .NotEmpty().WithMessage(AppMessages.SynchronizationStates_Color_Required);
        }

    }
}
