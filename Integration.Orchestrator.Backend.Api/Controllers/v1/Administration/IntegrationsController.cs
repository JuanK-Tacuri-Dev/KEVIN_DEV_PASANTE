using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class IntegrationsController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(IntegrationCreateRequest request)
        {
            return Ok(await _mediator.Send(new CreateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationCreateRequest>(request))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(IntegrationUpdateRequest request, Guid id)
        {
            return Ok(await _mediator.Send(new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(request), id)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteIntegrationCommandRequest(
                new IntegrationDeleteRequest { Id = id })));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _mediator.Send(
                new GetByIdIntegrationCommandRequest(
                    new IntegrationGetByIdRequest { Id = id })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(IntegrationGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedIntegrationCommandRequest(request)));
        }
    }
}
