using FluentValidation.TestHelper;
using Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.Validators;
using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Domain.Resources;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Application.Tests.Administrations.Handlers.Administration.Integration.Validators
{
    public class UpdateIntegrationCommandRequestValidatorTests
    {
        private readonly UpdateIntegrationCommandRequestValidator _validator;

        public UpdateIntegrationCommandRequestValidatorTests()
        {
            _validator = new UpdateIntegrationCommandRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(
                    new IntegrationUpdateRequest
                    {
                        Name = string.Empty
                    }), Guid.NewGuid());
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Integration.IntegrationRequest.Name)
                  .WithErrorMessage(AppMessages.Integration_Name_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Empty()
        {
            var model = new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(
                    new IntegrationUpdateRequest
                    {
                        Status = Guid.Empty
                    }), Guid.NewGuid());
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Integration.IntegrationRequest.Status)
                  .WithErrorMessage(AppMessages.Integration_Status_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Observations_Is_Empty()
        {
            var model = new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(
                    new IntegrationUpdateRequest
                    {
                        Observations = string.Empty
                    }), Guid.NewGuid());
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Integration.IntegrationRequest.Observations)
                  .WithErrorMessage(AppMessages.Integration_Observations_Required);
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var model = new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(
                    new IntegrationUpdateRequest
                    {
                        UserId = Guid.Empty
                    }), Guid.NewGuid());
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Integration.IntegrationRequest.UserId)
                  .WithErrorMessage(AppMessages.Integration_UserId_Required);
        }

        [Fact]
        public void Should_Have_Error_When_Process_Is_Empty()
        {
            var model = new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(
                    new IntegrationUpdateRequest
                    {
                        Process = new List<ProcessRequest> { }
                    }), Guid.NewGuid());
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Integration.IntegrationRequest.Process)
                  .WithErrorMessage(AppMessages.Integration_Process_Required);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Present()
        {
            var model = new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(
                    new IntegrationUpdateRequest
                    {
                        Name = "Valid Name",
                        Status = Guid.NewGuid(),
                        Observations = "Valid Observations",
                        UserId = Guid.NewGuid(),
                        Process = new List<ProcessRequest> 
                        {
                            new ProcessRequest
                            {
                                Id = Guid.NewGuid()
                            }
                        }
                    }), Guid.NewGuid());
            
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Integration.IntegrationRequest.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Integration.IntegrationRequest.Status);
            result.ShouldNotHaveValidationErrorFor(x => x.Integration.IntegrationRequest.Observations);
            result.ShouldNotHaveValidationErrorFor(x => x.Integration.IntegrationRequest.UserId);
            result.ShouldNotHaveValidationErrorFor(x => x.Integration.IntegrationRequest.Process);
        }
    }
}
