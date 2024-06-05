using FluentValidation.TestHelper;
using Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization.Validators;
using Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Validators
{
    public class UpdateSynchronizationCommandRequestValidatorTests
    {
        private readonly UpdateSynchronizationCommandRequestValidator _validator;

        public UpdateSynchronizationCommandRequestValidatorTests()
        {
            _validator = new UpdateSynchronizationCommandRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FranchiseId_Is_Empty()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>( new SynchronizationUpdateRequest 
            {
                FranchiseId = Guid.Empty,
            }), Guid.NewGuid());
            

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.FranchiseId)
                  .WithErrorMessage(AppMessages.Synchronization_FranchiseId_Required);
        }

        [Fact]
        public void Should_Not_Have_Error_When_FranchiseId_Is_Provided()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                FranchiseId = Guid.NewGuid()
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.FranchiseId);
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Empty()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Status)
                  .WithErrorMessage(AppMessages.Synchronization_Status_Required);
        }

   
        [Fact]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                Status = Guid.NewGuid()
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Status);
        }

        [Fact]
        public void Should_Have_Error_When_Observations_Is_Empty()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                Observations = ""
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Observations)
                  .WithErrorMessage(AppMessages.Synchronization_Observations_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Observations_Is_Too_Short()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                Observations = ""
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Observations)
                  .WithErrorMessage(AppMessages.Synchronization_Observations_MinimumSize);
        }

        [Fact]
        public void Should_Have_Error_When_Observations_Is_Too_Long()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                Observations = new string('a', 256)
            }), Guid.NewGuid()) ;

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Observations)
                  .WithErrorMessage(AppMessages.Synchronization_Observations_MaximumSize);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Observations_Is_Valid()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                Observations = "Valid observations"
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.Observations);
        }

        [Fact]
        public void Should_Have_Error_When_HourToExecute_Is_Null()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                HourToExecute = null
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.HourToExecute)
                  .WithErrorMessage(AppMessages.Synchronization_HourToExecute_Required);
        }

        [Fact]
        public void Should_Not_Have_Error_When_HourToExecute_Is_Valid()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                HourToExecute = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.HourToExecute);
        }
    }
}
