using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.ServerCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Server.Validators
{
    public class CreateServerCommandRequestValidator : AbstractValidator<CreateServerCommandRequest>
    {
        public CreateServerCommandRequestValidator()
        {
            RuleFor(request => request.Server.ServerRequest.Code)
            .NotEmpty().WithMessage(AppMessages.Server_Code_Required);

            RuleFor(request => request.Server.ServerRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Server_Name_Required);

            RuleFor(request => request.Server.ServerRequest.Type)
            .NotEmpty().WithMessage(AppMessages.Server_Type_Required);

            RuleFor(request => request.Server.ServerRequest.Url)
            .NotEmpty().WithMessage(AppMessages.Server_Url_Required);
        }
    }
}
