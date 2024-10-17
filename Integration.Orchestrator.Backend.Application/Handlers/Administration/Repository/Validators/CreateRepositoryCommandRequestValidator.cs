using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateRepositoryCommandRequestValidator : AbstractValidator<CreateRepositoryCommandRequest>
    {
        public CreateRepositoryCommandRequestValidator()
        {
            RuleFor(request => request.Repository.RepositoryRequest.UserName)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Repository.RepositoryRequest.Password)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Repository.RepositoryRequest.DatabaseName)
            .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

            RuleFor(request => request.Repository.RepositoryRequest.StatusId)
                .NotEmpty().WithMessage(AppMessages.Application_Validator_Required);

        }
    }
}
