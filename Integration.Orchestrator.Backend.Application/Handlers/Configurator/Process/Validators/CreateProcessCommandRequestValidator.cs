using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Process.ProcessCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Process.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateProcessCommandRequestValidator : AbstractValidator<CreateProcessCommandRequest>
    {
        public CreateProcessCommandRequestValidator()
        {
            RuleFor(request => request.Process.ProcessRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
            .MaximumLength(100).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 100));

            RuleFor(request => request.Process.ProcessRequest.Description)
            .MaximumLength(250).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 250));

            RuleFor(request => request.Process.ProcessRequest.TypeId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Process.ProcessRequest.ConnectionId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Process.ProcessRequest.StatusId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Process.ProcessRequest.Entities)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
