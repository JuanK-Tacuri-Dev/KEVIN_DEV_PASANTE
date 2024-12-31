using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Server.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateServerCommandRequestValidator : AbstractValidator<UpdateServerCommandRequest>
    {
        public UpdateServerCommandRequestValidator()
        {
            RuleFor(request => request.Server.ServerRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Server.ServerRequest.Url)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
