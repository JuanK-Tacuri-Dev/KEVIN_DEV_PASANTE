using FluentValidation.TestHelper;
using Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.Validators;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationStatusCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Synchronization.Validators
{
    public class CreateSynchronizationStatesCommandRequestValidatorTests
    {
        private readonly CreateSynchronizationStatusCommandRequestValidator _validator;

        public CreateSynchronizationStatesCommandRequestValidatorTests()
        {
            _validator = new CreateSynchronizationStatusCommandRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Null_Or_Empty()
        {
            var request = new CreateSynchronizationStatusCommandRequest(
                new SynchronizationStatusBasicInfoRequest<SynchronizationStatusCreateRequest>(
                    new SynchronizationStatusCreateRequest
                    {
                        Key = string.Empty,
                        Text = "Active",
                        Color = "Green",
                        Background = "#E2F7E2"
                    }));
            
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Key)
                  .WithErrorMessage(AppMessages.SynchronizationStates_Name_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Code_Is_Null_Or_Empty()
        {
            var request = new CreateSynchronizationStatusCommandRequest(
                new SynchronizationStatusBasicInfoRequest<SynchronizationStatusCreateRequest>(
                    new SynchronizationStatusCreateRequest
                    {
                        Key = "Cancelado",
                        Text = string.Empty,
                        Color = "F77D7D",
                        Background = "#E2F7E2"
                    }));
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Text)
                  .WithErrorMessage(AppMessages.SynchronizationStates_Code_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Color_Is_Null_Or_Empty()
        {
            var request = new CreateSynchronizationStatusCommandRequest(
                new SynchronizationStatusBasicInfoRequest<SynchronizationStatusCreateRequest>(
                    new SynchronizationStatusCreateRequest
                    {
                        Key = "Cancelado",
                        Text = "canceled",
                        Color = string.Empty,
                        Background = "#E2F7E2"
                    }));
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Color)
                  .WithErrorMessage(AppMessages.SynchronizationStates_Color_Required);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Properties_Are_Valid()
        {
            var request = new CreateSynchronizationStatusCommandRequest(
                new SynchronizationStatusBasicInfoRequest<SynchronizationStatusCreateRequest>(
                    new SynchronizationStatusCreateRequest
                    {
                        Key = "State Name",
                        Text = "Active",
                        Color = "Green"
                    }));
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Key);
            result.ShouldNotHaveValidationErrorFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Text);
            result.ShouldNotHaveValidationErrorFor(request => request.SynchronizationStatus.SynchronizationStatesRequest.Color);
        }
    }
}
