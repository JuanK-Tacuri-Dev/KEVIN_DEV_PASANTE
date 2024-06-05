using FluentValidation.TestHelper;
using Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization.Validators;
using Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Validators
{
    public class CreateSynchronizationCommandRequestValidatorTests
    {
        private readonly CreateSynchronizationCommandRequestValidator _validator;

        public CreateSynchronizationCommandRequestValidatorTests()
        {
            _validator = new CreateSynchronizationCommandRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FranchiseId_Is_Empty()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>( new SynchronizationCreateRequest 
            {
                FranchiseId = Guid.Empty,
            }));
            

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.FranchiseId)
                  .WithErrorMessage(AppMessages.Synchronization_FranchiseId_Required);
        }

        [Fact]
        public void Should_Not_Have_Error_When_FranchiseId_Is_Provided()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
                FranchiseId = Guid.NewGuid()
            }));

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.FranchiseId);
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Empty()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
            }));

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Status)
                  .WithErrorMessage(AppMessages.Synchronization_Status_Required);
        }

   
        [Fact]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
                Status = Guid.NewGuid()
            }));

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Status);
        }

        [Fact]
        public void Should_Have_Error_When_Observations_Is_Empty()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
                Observations = ""
            }));

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Observations)
                  .WithErrorMessage(AppMessages.Synchronization_Observations_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Observations_Is_Too_Short()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
                Observations = ""
            }));

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Observations)
                  .WithErrorMessage(AppMessages.Synchronization_Observations_MinimumSize);
        }

        [Fact]
        public void Should_Have_Error_When_Observations_Is_Too_Long()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
                Observations = new string('a', 256)
            })) ;

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Observations)
                  .WithErrorMessage(AppMessages.Synchronization_Observations_MaximumSize);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Observations_Is_Valid()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
                Observations = "Valid observations"
            }));

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Observations);
        }

        [Fact]
        public void Should_Have_Error_When_HourToExecute_Is_Null()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
                HourToExecute = null
            }));

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.HourToExecute)
                  .WithErrorMessage(AppMessages.Synchronization_HourToExecute_Required);
        }

        [Fact]
        public void Should_Not_Have_Error_When_HourToExecute_Is_Valid()
        {
            var model = new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(new SynchronizationCreateRequest
            {
                HourToExecute = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            }));

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.HourToExecute);
        }
    }
}
