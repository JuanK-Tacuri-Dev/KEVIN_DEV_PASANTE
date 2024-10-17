using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateServerCommandRequestValidator : AbstractValidator<CreateServerCommandRequest>
    {
        public CreateServerCommandRequestValidator()
        {
            RuleFor(request => request.Server.ServerRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Server.ServerRequest.Url)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
