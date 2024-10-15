using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Integration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Integration.IntegrationCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurador
{
    [Route("api/v1/integrations/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class IntegrationController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(IntegrationCreateRequest request)
        {
            return Ok((await _mediator.Send(new CreateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(IntegrationUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(new UpdateIntegrationCommandRequest(
                new IntegrationBasicInfoRequest<IntegrationUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(new DeleteIntegrationCommandRequest(
                new IntegrationDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdIntegrationCommandRequest(
                    new IntegrationGetByIdRequest { Id = id }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(IntegrationGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedIntegrationCommandRequest(request))).Message);
        }
    }
}
