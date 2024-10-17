using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationStatusCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateSynchronizationStatusCommandRequestValidator : AbstractValidator<CreateSynchronizationStatusCommandRequest>
    {
        public CreateSynchronizationStatusCommandRequestValidator()
        {
            RuleFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Key)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Text)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Color)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Background)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }

    }
}
