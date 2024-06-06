using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administrations.SynchronizationStates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.SynchronizationStates.SynchronizationStatesStatesCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administrations
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
            return Ok(await _mediator.Send(new CreateSynchronizationStatesCommandRequest(request)));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(SynchronizationStatesGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedSynchronizationStatesCommandRequest(request)));
        }
    }
}
