﻿using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Globalization;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.Validators
{
    public class CreateSynchronizationCommandRequestValidator : AbstractValidator<CreateSynchronizationCommandRequest>
    {
        public CreateSynchronizationCommandRequestValidator()
        {

            RuleFor(request => request.Synchronization.SynchronizationRequest.Status)
            .NotEmpty().WithMessage(AppMessages.Synchronization_Status_Required);

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
