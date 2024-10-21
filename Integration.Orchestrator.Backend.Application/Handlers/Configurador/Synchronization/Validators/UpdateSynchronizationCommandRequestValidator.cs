using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Helper;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurador.Synchronization.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateSynchronizationCommandRequestValidator : AbstractValidator<UpdateSynchronizationCommandRequest>
    {
        public UpdateSynchronizationCommandRequestValidator()
        {
            RuleFor(request => request.Id)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Synchronization.SynchronizationRequest.StatusId)
           .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Synchronization.SynchronizationRequest.Integrations)
            .NotNull().WithMessage(AppMessages.Application_Validator_Required)
            .NotEmpty().WithMessage(AppMessages.Synchronization_Integrations_MinimumValue);
        }

        private bool BeAValidDateTime(string dateTimeString)
        {
            return DateTime.TryParseExact(dateTimeString, ConfigurationSystem.DateTimeDefault(), CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
