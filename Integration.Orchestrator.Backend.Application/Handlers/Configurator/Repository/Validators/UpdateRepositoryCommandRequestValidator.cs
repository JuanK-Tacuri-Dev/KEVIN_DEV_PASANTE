using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Repository.Validators
{
    [ExcludeFromCodeCoverage]
    public class UpdateRepositoryCommandRequestValidator : AbstractValidator<UpdateRepositoryCommandRequest>
    {
        public UpdateRepositoryCommandRequestValidator()
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
