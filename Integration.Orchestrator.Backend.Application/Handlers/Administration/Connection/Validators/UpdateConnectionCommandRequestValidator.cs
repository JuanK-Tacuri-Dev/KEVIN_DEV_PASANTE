using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Connection.Validators
{
    public class UpdateConnectionCommandRequestValidator : AbstractValidator<UpdateConnectionCommandRequest>
    {
        public UpdateConnectionCommandRequestValidator()
        {
            RuleFor(request => request.Connection.ConnectionRequest.ServerId)
                .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Connection.ConnectionRequest.AdapterId)
                .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Connection.ConnectionRequest.RepositoryId)
                .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Connection.ConnectionRequest.Name)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required)
            .MaximumLength(100).WithMessage(string.Format(AppMessages.Application_Validator_MaxLength, 100));

            RuleFor(request => request.Connection.ConnectionRequest.StatusId)
                .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);
        }
    }
}
