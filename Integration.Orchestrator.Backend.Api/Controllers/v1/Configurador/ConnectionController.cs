using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Connection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Connection.ConnectionCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurador
{
    [Route("api/v1/connections/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class ConnectionController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(ConnectionCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateConnectionCommandRequest(
                    new ConnectionBasicInfoRequest<ConnectionCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ConnectionUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(new UpdateConnectionCommandRequest(
                new ConnectionBasicInfoRequest<ConnectionUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeleteConnectionCommandRequest(
                    new ConnectionDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdConnectionCommandRequest(
                    new ConnectionGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok((await _mediator.Send(
                new GetByCodeConnectionCommandRequest(
                    new ConnectionGetByCodeRequest { Code = code }))).Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok((await _mediator.Send(
                new GetByTypeConnectionCommandRequest(
                    new ConnectionGetByTypeRequest { Type = type }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(ConnectionGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedConnectionCommandRequest(request))).Message);
        }
    }
}
