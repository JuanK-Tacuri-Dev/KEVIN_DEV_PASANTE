using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Globalization;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Synchronization.Validators
{
    public class CreateSynchronizationCommandRequestValidator : AbstractValidator<CreateSynchronizationCommandRequest>
    {
        public CreateSynchronizationCommandRequestValidator()
        {

            RuleFor(request => request.Synchronization.SynchronizationRequest.StatusId)
            .NotEmpty().WithMessage(AppMessages.Synchronization_Status_Required);

            RuleFor(request => request.Synchronization.SynchronizationRequest.Integrations)
            .NotNull().WithMessage(AppMessages.Synchronization_Integrations_Required)
            .NotEmpty().WithMessage(AppMessages.Synchronization_Integrations_MinimumValue);
        }

        private bool BeAValidDateTime(string dateTimeString)
        {
            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
            return DateTime.TryParseExact(dateTimeString, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
