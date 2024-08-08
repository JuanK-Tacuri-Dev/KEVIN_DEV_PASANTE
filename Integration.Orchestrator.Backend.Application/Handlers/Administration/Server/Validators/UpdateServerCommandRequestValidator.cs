using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.Validators
{
    public class UpdateServerCommandRequestValidator : AbstractValidator<UpdateServerCommandRequest>
    {
        public UpdateServerCommandRequestValidator()
        {
            RuleFor(request => request.Server.ServerRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Server.ServerRequest.TypeServerId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Server.ServerRequest.Url)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
