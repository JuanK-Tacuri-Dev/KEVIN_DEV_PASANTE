using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Synchronization.SynchronizationStatusCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/synchronizationStates/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class SynchronizationStatusController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(SynchronizationStatusCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateSynchronizationStatusCommandRequest(
                    new SynchronizationStatusBasicInfoRequest<SynchronizationStatusCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(SynchronizationStatusUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(
                new UpdateSynchronizationStatusCommandRequest(
                    new SynchronizationStatusBasicInfoRequest<SynchronizationStatusUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeleteSynchronizationStatusCommandRequest(
                    new SynchronizationStatusDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdSynchronizationStatusCommandRequest(
                    new SynchronizationStatusGetByIdRequest { Id = id }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(SynchronizationStatusGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedSynchronizationStatusCommandRequest(request))).Message);
        }
    }
}
