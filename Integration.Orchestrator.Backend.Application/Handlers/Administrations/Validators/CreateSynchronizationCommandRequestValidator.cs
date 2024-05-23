using FluentValidation;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.AdministrationsCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Validators
{
    internal class CreateSynchronizationCommandRequestValidator : AbstractValidator<CreateSynchronizationCommandRequest>
    {
        public CreateSynchronizationCommandRequestValidator()
        {
            RuleFor(request => request.Synchronization.SynchronizationRequest.FranchiseId)
            .NotEmpty()
            .WithMessage("AppMessages.Synchronization_FranchiseId_Required");

            RuleFor(request => request.Synchronization.SynchronizationRequest.Status)
           .NotNull()
           .WithMessage("AppMessages.Synchronization_Status_Required")
           .MinimumLength(1).MinimumLength(1)
           .WithMessage("AppMessages.Synchronization_Status_Lenght");

            RuleFor(request => request.Synchronization.SynchronizationRequest.Observations)
           .MinimumLength(1).MinimumLength(255)
           .WithMessage("AppMessages.Synchronization_Observations_Lenght");

        }
    }
}
