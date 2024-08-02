using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.Validators
{
    public class CreateRepositoryCommandRequestValidator : AbstractValidator<CreateRepositoryCommandRequest>
    {
        public CreateRepositoryCommandRequestValidator()
        {
            RuleFor(request => request.Repository.RepositoryRequest.Code)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Repository.RepositoryRequest.UserName)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Repository.RepositoryRequest.Password)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Repository.RepositoryRequest.DataBaseName)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Repository.RepositoryRequest.AuthTypeId)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Repository.RepositoryRequest.StatusId)
                .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

        }
    }
}
