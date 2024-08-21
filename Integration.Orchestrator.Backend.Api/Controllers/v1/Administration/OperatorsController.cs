using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Operator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Operator.OperatorCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class OperatorsController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(OperatorCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateOperatorCommandRequest(
                    new OperatorBasicInfoRequest<OperatorCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(OperatorUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(new UpdateOperatorCommandRequest(
                new OperatorBasicInfoRequest<OperatorUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(new DeleteOperatorCommandRequest(
                new OperatorDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdOperatorCommandRequest(
                    new OperatorGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok((await _mediator.Send(
                new GetByCodeOperatorCommandRequest(
                    new OperatorGetByCodeRequest { Code = code }))).Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(Guid typeId)
        {
            return Ok((await _mediator.Send(
                new GetByTypeOperatorCommandRequest(
                    new OperatorGetByTypeRequest { TypeId = typeId }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(OperatorGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedOperatorCommandRequest(request))).Message);
        }
    }
}
