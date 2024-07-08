using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class SynchronizationStatesController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(SynchronizationStatesCreateRequest request)
        {
            return Ok(await _mediator.Send(
                new CreateSynchronizationStatesCommandRequest(
                    new SynchronizationStatesBasicInfoRequest<SynchronizationStatesCreateRequest>(request))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SynchronizationStatesUpdateRequest request, Guid id)
        {
            return Ok(await _mediator.Send(
                new UpdateSynchronizationStatesCommandRequest(
                    new SynchronizationStatesBasicInfoRequest<SynchronizationStatesUpdateRequest>(request), id)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(
                new DeleteSynchronizationStatesCommandRequest(
                    new SynchronizationStatesDeleteRequest { Id = id })));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _mediator.Send(
                new GetByIdSynchronizationStatesCommandRequest(
                    new SynchronizationStatesGetByIdRequest { Id = id })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(SynchronizationStatesGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedSynchronizationStatesCommandRequest(request))).Message);
        }
    }
}
