using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Globalization;
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
           .NotEmpty().WithMessage(AppMessages.Synchronization_Status_Required);

            RuleFor(request => request.Synchronization.SynchronizationRequest.Observations)
            .NotEmpty().WithMessage(AppMessages.Synchronization_Observations_Required)
            .MinimumLength(1).WithMessage(AppMessages.Synchronization_Observations_MinimumSize)
            .MaximumLength(255).WithMessage(AppMessages.Synchronization_Observations_MaximumSize);

            RuleFor(request => request.Synchronization.SynchronizationRequest.Integrations)
            .NotNull().WithMessage(AppMessages.Synchronization_Integrations_Required)
            .NotEmpty().WithMessage(AppMessages.Synchronization_Integrations_MinimumValue);

            RuleFor(request => request.Synchronization.SynchronizationRequest.HourToExecute)
            .NotNull().WithMessage(AppMessages.Synchronization_HourToExecute_Required)
            .Must(BeAValidDateTime).WithMessage(AppMessages.Synchronization_HourToExecute_Invalid);
        }

        private bool BeAValidDateTime(string dateTimeString)
        {
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
            return DateTime.TryParseExact(dateTimeString, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
