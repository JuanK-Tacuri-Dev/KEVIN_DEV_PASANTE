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
            .NotEmpty().WithMessage(AppMessages.Repository_Code_Required);

            RuleFor(request => request.Repository.RepositoryRequest.UserName)
            .NotEmpty().WithMessage(AppMessages.Repository_User_Required);

            RuleFor(request => request.Repository.RepositoryRequest.Password)
            .NotEmpty().WithMessage(AppMessages.Repository_Password_Required);

            RuleFor(request => request.Repository.RepositoryRequest.DataBaseName)
            .NotEmpty().WithMessage(AppMessages.Repository_ServerId_Required);

            RuleFor(request => request.Repository.RepositoryRequest.AuthTypeId)
            .NotEmpty().WithMessage(AppMessages.Repository_AdapterId_Required);

            RuleFor(request => request.Repository.RepositoryRequest.StatusId)
                .NotEmpty().WithMessage(AppMessages.Repository_AdapterId_Required);

        }
    }
}
