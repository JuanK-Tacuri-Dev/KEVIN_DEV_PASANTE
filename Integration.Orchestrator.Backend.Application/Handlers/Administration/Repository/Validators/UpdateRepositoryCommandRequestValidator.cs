using FluentValidation;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Administration.Repository.Validators
{
    public class UpdateRepositoryCommandRequestValidator : AbstractValidator<UpdateRepositoryCommandRequest>
    {
        public UpdateRepositoryCommandRequestValidator()
        {
            RuleFor(request => request.Repository.RepositoryRequest.Code)
            .NotEmpty().WithMessage(AppMessages.Repository_Code_Required);

            RuleFor(request => request.Repository.RepositoryRequest.Port)
            .NotEmpty().WithMessage(AppMessages.Repository_Port_Required);

            RuleFor(request => request.Repository.RepositoryRequest.User)
            .NotEmpty().WithMessage(AppMessages.Repository_User_Required);

            RuleFor(request => request.Repository.RepositoryRequest.Password)
            .NotEmpty().WithMessage(AppMessages.Repository_Password_Required);

            RuleFor(request => request.Repository.RepositoryRequest.ServerId)
            .NotEmpty().WithMessage(AppMessages.Repository_ServerId_Required);

            RuleFor(request => request.Repository.RepositoryRequest.AdapterId)
            .NotEmpty().WithMessage(AppMessages.Repository_AdapterId_Required);


        }
    }
}
