using FluentValidation.TestHelper;
using Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.Validators;
using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Synchronization.Validators
{
    public class UpdateSynchronizationCommandRequestValidatorTests
    {
        private readonly UpdateSynchronizationCommandRequestValidator _validator;

        public UpdateSynchronizationCommandRequestValidatorTests()
        {
            _validator = new UpdateSynchronizationCommandRequestValidator();
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
            result.ShouldHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.StatusId)
                  .WithErrorMessage(AppMessages.Synchronization_Status_Required);
        }


        [Fact]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var model = new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(new SynchronizationUpdateRequest
            {
                StatusId = Guid.NewGuid()
            }), Guid.NewGuid());

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.Synchronization.SynchronizationRequest.StatusId);
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
