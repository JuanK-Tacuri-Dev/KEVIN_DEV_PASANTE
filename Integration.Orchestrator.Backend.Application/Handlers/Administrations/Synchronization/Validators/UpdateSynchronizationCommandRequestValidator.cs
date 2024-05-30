using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization.Validators
{
    public class UpdateSynchronizationCommandRequestValidator : AbstractValidator<UpdateSynchronizationCommandRequest>
    {
        public UpdateSynchronizationCommandRequestValidator()
        {
            RuleFor(request => request.Id)
            .NotEmpty().WithMessage(AppMessages.Synchronization_Id_Required);

            RuleFor(request => request.Synchronization.SynchronizationRequest.FranchiseId)
            .NotEmpty().WithMessage(AppMessages.Synchronization_FranchiseId_Required);

            RuleFor(request => request.Synchronization.SynchronizationRequest.Status)
                .Cascade(CascadeMode.Stop)
           .NotEmpty().WithMessage(AppMessages.Synchronization_Status_Required)
           .MinimumLength(1).WithMessage(AppMessages.Synchronization_Status_MinimumSize)
           .MaximumLength(1).WithMessage(AppMessages.Synchronization_Status_MaximumSize);

            RuleFor(request => request.Synchronization.SynchronizationRequest.Observations)
                .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(AppMessages.Synchronization_Observations_Required)
           .MinimumLength(1).WithMessage(AppMessages.Synchronization_Observations_MinimumSize)
           .MaximumLength(255).WithMessage(AppMessages.Synchronization_Observations_MaximumSize);

            RuleFor(request => request.Synchronization.SynchronizationRequest.HourToExecute)
                .Cascade(CascadeMode.Stop)
                 .NotNull().WithMessage(AppMessages.Synchronization_HourToExecute_Required)
                .Must(BeAValidDateTime).WithMessage(AppMessages.Synchronization_HourToExecute_Invalid)
           ;
        }

        private bool BeAValidDateTime(DateTime dateTime)
        {
            return dateTime != default;
        }
    }
}
