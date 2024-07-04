using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Status;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class StateController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(StatusCreateRequest request)
        {
            return Ok(await _mediator.Send(
                new CreateStatusCommandRequest(
                    new StatusBasicInfoRequest<StatusCreateRequest>(request))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(StatusUpdateRequest request, Guid id)
        {
            return Ok(await _mediator.Send(
                new UpdateStatusCommandRequest(
                    new StatusBasicInfoRequest<StatusUpdateRequest>(request), id)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(
                new DeleteStatusCommandRequest(
                    new StatusDeleteRequest { Id = id })));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _mediator.Send(
                new GetByIdStatusCommandRequest(
                    new StatusGetByIdRequest { Id = id })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(StatusGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(
                new GetAllPaginatedStatusCommandRequest(request)));
        }
    }
}
